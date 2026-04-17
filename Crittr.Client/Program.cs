using Crittr.App;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBase = await ResolveApiBaseAsync(builder);
builder.Services.AddCrittrApp(apiBase);

await builder.Build().RunAsync();

static async Task<string> ResolveApiBaseAsync(WebAssemblyHostBuilder builder)
{
    try
    {
        using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
        await using var stream = await http.GetStreamAsync("appsettings.json");
        builder.Configuration.AddJsonStream(stream);
    }
    catch
    {
        // Optional file (e.g. first run); fall through to default
    }

    var url = builder.Configuration["ApiBaseUrl"];
    if (string.IsNullOrWhiteSpace(url))
        url = "https://localhost:7282/";

    return url.TrimEnd('/') + "/";
}
