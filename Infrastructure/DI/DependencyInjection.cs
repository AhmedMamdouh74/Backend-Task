using Application.Services;
using Domain.Repos;
using Domian.ReposContract;
using Infrastructure.Repos;
using Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System.Collections.Concurrent;



namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton(new ConcurrentDictionary<string, CountryBlock>());
        services.AddScoped<ICountryRepo, CountryRepo>();
        services.AddHostedService<TemporalBlockCleanupService>();
        services.AddHttpClient<GeoLocationService>();
        services.AddScoped<IGeoLocationService, GeoLocationService>();
        services.AddSingleton<IBlockedAttemptLogRepo, BlockedAttemptLogRepo>();


        return services;
    }
}
