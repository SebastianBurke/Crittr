using Microsoft.EntityFrameworkCore;
using ReptileCare.Server.Data;
using ReptileCare.Server.Services;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.Models;
using IReptileService = ReptileCare.Server.Services.Interfaces.IReptileService;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IReptileService, ReptileService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

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


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(); // must be before UseAuthorization
app.UseAuthorization();

// This maps all [ApiController] controllers
app.MapControllers();

// Seed dummy data if needed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!db.Reptiles.Any())
    {
        db.Reptiles.Add(new Reptile
        {
            Id = 1,
            Name = "Leo",
            Species = "Leopard Gecko"
        });
        db.SaveChanges();
    }
}

app.Run();