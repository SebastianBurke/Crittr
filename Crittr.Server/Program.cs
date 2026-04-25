using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Crittr.Server.Data;
using Crittr.Server.Models;
using Crittr.Server.Services;
using Crittr.Server.Services.Interfaces;
using Crittr.Shared.Models;
using ICritterService = Crittr.Server.Services.Interfaces.ICritterService;

var builder = WebApplication.CreateBuilder(args);

// In Development, bind the URLs the Blazor client expects (see Crittr.App / launchSettings https profile).
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://0.0.0.0:5099", "https://localhost:7282");
}

// JWT signing key MUST be supplied via env var (Jwt__Key) or `dotnet user-secrets`.
// We do NOT accept the empty placeholder from appsettings.json — fail fast at startup.
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "Jwt:Key is not configured. Provide it via env var (Jwt__Key) or user-secrets " +
        "(`dotnet user-secrets set \"Jwt:Key\" <value> --project Crittr.Server`). " +
        "Generate a strong key with: openssl rand -base64 48");
}

// ----------------- Services -----------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<ISheddingService, SheddingService>();
builder.Services.AddScoped<IFeedingService, FeedingService>();
builder.Services.AddScoped<IHealthAnalyticsEngine, HealthAnalyticsEngine>();
builder.Services.AddScoped<IEnvironmentService, EnvironmentService>();
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IScheduledTaskService, ScheduledTaskService>();
builder.Services.AddScoped<ICritterService, CritterService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<IEnclosureService, EnclosureService>();
builder.Services.AddSingleton<SpeciesCatalogService>();
builder.Services.AddScoped<EnclosureCohabitationService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// ASP.NET Identity with strict password and lockout policy.
builder.Services.AddIdentity<AppUser, IdentityRole>(o =>
{
    o.Password.RequiredLength = 12;
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = true;
    o.Password.RequireUppercase = true;
    o.Password.RequireNonAlphanumeric = true;

    o.Lockout.MaxFailedAccessAttempts = 5;
    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    o.Lockout.AllowedForNewUsers = true;

    o.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();


// CORS for Blazor WASM (Crittr.Client launchSettings: https on 7110, http on 5267 — scheme+port must match exactly).
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                    return false;
                if (uri.IsLoopback)
                    return true;
                // Allow private-network addresses for LAN testing (iPhone, emulators, etc.)
                if (uri.HostNameType == UriHostNameType.IPv4 &&
                    System.Net.IPAddress.TryParse(uri.Host, out var ip))
                {
                    var b = ip.GetAddressBytes();
                    return b[0] == 10 ||
                           (b[0] == 172 && b[1] >= 16 && b[1] <= 31) ||
                           (b[0] == 192 && b[1] == 168) ||
                           // Tailscale CGNAT range is 100.64.0.0/10 (bytes 64-127), not all of 100.x.x.x.
                           (b[0] == 100 && b[1] >= 64 && b[1] <= 127);
                }
                return false;
            });
        }
        else
        {
            policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? ["https://localhost:7110"]);
        }

        policy.AllowAnyHeader().AllowAnyMethod();
    });
});

// Single JWT Bearer registration (the previous double-AddAuthentication setup was redundant).
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
            }
        };
    });

builder.Services.AddAuthorization();

// Per-IP rate limiting on auth endpoints (login + register policies are applied via [EnableRateLimiting] attributes).
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddPolicy("register", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromHours(1),
                QueueLimit = 0
            }));
});


var app = builder.Build();

// Apply pending migrations and enable SQLite WAL on startup.
// MigrateAsync is idempotent; WAL is persisted at the DB-file level so this is effectively a one-time op.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await db.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;");
}

// Global exception handler — never leak stack traces to clients in production.
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var traceId = context.TraceIdentifier;

        object problem = app.Environment.IsDevelopment()
            ? new { type = "InternalServerError", title = "Internal server error", traceId, detail = feature?.Error.Message }
            : new { type = "InternalServerError", title = "Internal server error", traceId };

        await context.Response.WriteAsJsonAsync(problem);
    });
});

// Security headers middleware (runs before auth so anonymous responses are also hardened).
app.Use(async (ctx, next) =>
{
    var h = ctx.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["X-Frame-Options"] = "DENY";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    h["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    if (!app.Environment.IsDevelopment())
        h["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseCors(); // must be before UseAuthorization
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Demo accounts and sample data are ONLY seeded in Development.
// Production must NOT have well-known credentials seeded automatically.
if (app.Environment.IsDevelopment())
{
    var sampleDataOwnerId = await SeedDemoUsers(app);

    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync(sampleDataOwnerId);
}

app.Run();

/// <summary>
/// Ensures demo accounts exist (development only). Sample data attaches to <c>demo@crittr.ca</c>.
/// </summary>
static async Task<string> SeedDemoUsers(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await EnsureDemoUserAsync(userManager, logger, "demo@crittr.ca", "Demo123!");
    await EnsureDemoUserAsync(userManager, logger, "empty@crittr.ca", "Demo123!");

    var owner = await userManager.FindByEmailAsync("demo@crittr.ca");
    if (owner is null)
        throw new InvalidOperationException("demo@crittr.ca was not found after seeding.");

    return owner.Id;

    static async Task EnsureDemoUserAsync(UserManager<AppUser> userManager, ILogger logger, string email, string password)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            logger.LogError("Failed to seed demo user {Email}: {Errors}",
                email, string.Join(", ", result.Errors.Select(e => e.Description)));
            throw new InvalidOperationException(
                $"Failed to seed demo user {email} (see logs for details).");
        }
    }
}
