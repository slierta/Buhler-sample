using Microsoft.Extensions.DependencyInjection;
using Sample.Contracts;

namespace Sample.Services.DI;

/// <summary>
/// Register the <see cref="ITruckFinder"/> implementation
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="ITruckFinder"/> implementation used to query the sf soda service
    /// </summary>
    /// <param name="services"></param>
    public static void AddTruckFinder(this IServiceCollection services)
    {
        services.AddTransient<ITruckFinder, TruckFinder>();
    }
}