using Microsoft.EntityFrameworkCore;
using ReptileCare.Server.Services;
using ReptileCare.Server.Services.Interfaces;
using ReptileCare.Shared.Data;
using ReptileCare.Shared.Models;
using IReptileService = ReptileCare.Server.Services.Interfaces.IReptileService;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-memory DB setup (for now)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("ReptileCareDB"));

// Dependency Injection
builder.Services.AddScoped<IReptileService, ReptileService>();
builder.Services.AddScoped<IHealthAnalyticsEngine, HealthAnalyticsEngine>();

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