using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
// Without this, `dotnet run` may use the "http" profile (5099 only) and the WASM app cannot reach https://localhost:7282.
if (builder.Environment.IsDevelopment())
{
    // Bind HTTP to all interfaces so LAN devices (phones, emulators) can reach the API.
    // HTTPS stays localhost-only (dev cert is not trusted on remote devices).
    builder.WebHost.UseUrls("http://0.0.0.0:5099", "https://localhost:7282");
}

// Register services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddIdentity<AppUser, IdentityRole>()
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
			   b[0] == 100;
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

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

        // 👇 Add this to prevent redirecting to /Account/Login
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


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseCors(); // must be before UseAuthorization

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var sampleDataOwnerId = await SeedDemoUsers(app);

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync(sampleDataOwnerId);
    await seeder.FixEnclosureTypesAsync();
}

app.Run();

/// <summary>
/// Ensures documented demo accounts exist. Sample critters/enclosures attach to <c>demo@critterapp.com</c>.
/// </summary>
static async Task<string> SeedDemoUsers(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    await EnsureDemoUserAsync(userManager, "demo@critterapp.com", "Demo123!");
    await EnsureDemoUserAsync(userManager, "demo@reptilecare.com", "Demo123!");
    await EnsureDemoUserAsync(userManager, "demo@demo.com", "Password123!");

    var owner = await userManager.FindByEmailAsync("demo@critterapp.com");
    if (owner is null)
        throw new InvalidOperationException("demo@critterapp.com was not found after seeding.");

    return owner.Id;

    static async Task EnsureDemoUserAsync(UserManager<AppUser> userManager, string email, string password)
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
            throw new Exception(
                $"Failed to seed {email}: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
