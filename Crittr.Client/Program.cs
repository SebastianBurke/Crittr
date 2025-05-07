using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Crittr.Client;
using Crittr.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7282/")
});
builder.Services.AddScoped<FeedingService>();
builder.Services.AddScoped<SheddingService>();
builder.Services.AddScoped<MeasurementService>();
builder.Services.AddScoped<HealthService>();
builder.Services.AddScoped<ScheduledTaskService>();
builder.Services.AddScoped<AuthorizedHandler>();
builder.Services.AddScoped<EnclosureService>();
builder.Services.AddScoped<SpeciesService>();

builder.Services.AddHttpClient<CritterService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/");
}).AddHttpMessageHandler<AuthorizedHandler>();

builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/");
});

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();