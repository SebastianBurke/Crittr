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

// Register services
builder.Services.AddControllers();
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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// CORS for Blazor client
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7110") // match Blazor client port
            .AllowAnyHeader()
            .AllowAnyMethod();
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


app.UseHttpsRedirection();
app.UseCors(); // must be before UseAuthorization

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var userId = await SeedDemoUser(app);

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync(userId);
}

app.Run();

// Seeding helper method
async Task<string> SeedDemoUser(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    var demoEmail = "demo@critterapp.com";
    var demoPassword = "Demo123!";

    var user = await userManager.FindByEmailAsync(demoEmail);
    if (user != null)
        return user.Id;

    var newUser = new AppUser
    {
        UserName = demoEmail,
        Email = demoEmail,
        EmailConfirmed = true
    };

    var result = await userManager.CreateAsync(newUser, demoPassword);
    if (!result.Succeeded)
    {
        throw new Exception("Failed to seed demo user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    return newUser.Id;
}