using Blazored.LocalStorage;
using Crittr.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crittr.App;

public static class AppClientServiceCollectionExtensions
{
    /// <summary>
    /// Registers shared Blazor UI services (HTTP APIs, local storage) for WebAssembly and MAUI Blazor Hybrid hosts.
    /// </summary>
    public static IServiceCollection AddCrittrApp(this IServiceCollection services, string apiBaseUri)
    {
        services.AddScoped<AuthorizedHandler>();

        services.AddHttpClient<SpeciesService>(client => client.BaseAddress = new Uri(apiBaseUri))
            .AddHttpMessageHandler<AuthorizedHandler>();

        services.AddHttpClient<CritterService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUri);
        }).AddHttpMessageHandler<AuthorizedHandler>();

        services.AddHttpClient<FeedingService>(client => client.BaseAddress = new Uri(apiBaseUri))
            .AddHttpMessageHandler<AuthorizedHandler>();

        services.AddHttpClient<SheddingService>(client => client.BaseAddress = new Uri(apiBaseUri))
            .AddHttpMessageHandler<AuthorizedHandler>();

        services.AddHttpClient<MeasurementService>(client => client.BaseAddress = new Uri(apiBaseUri))
            .AddHttpMessageHandler<AuthorizedHandler>();

        services.AddHttpClient<HealthService>(client => client.BaseAddress = new Uri(apiBaseUri))
            .AddHttpMessageHandler<AuthorizedHandler>();

        services.AddHttpClient<ScheduledTaskService>(client => client.BaseAddress = new Uri(apiBaseUri))
            .AddHttpMessageHandler<AuthorizedHandler>();

        services.AddHttpClient<EnclosureService>(client => client.BaseAddress = new Uri(apiBaseUri))
            .AddHttpMessageHandler<AuthorizedHandler>();

        services.AddHttpClient<AuthService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUri);
        });

        services.AddBlazoredLocalStorage();

        return services;
    }
}
