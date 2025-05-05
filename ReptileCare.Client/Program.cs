using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ReptileCare.Client;
using ReptileCare.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7282/")
});
builder.Services.AddScoped<ReptileService>();
builder.Services.AddScoped<FeedingService>();
builder.Services.AddScoped<SheddingService>();
builder.Services.AddScoped<MeasurementService>();
builder.Services.AddScoped<HealthService>();
builder.Services.AddScoped<ScheduledTaskService>();

await builder.Build().RunAsync();