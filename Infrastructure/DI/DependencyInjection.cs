using Application.Services;
using Domain.Repos;
using Domian.ReposContract;
using Infrastructure.Repos;
using Infrastructure.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System.Collections.Concurrent;

namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton(new ConcurrentDictionary<string, CountryBlock>());
        services.AddSingleton<ICountryRepo, CountryRepo>();
        services.AddHostedService<TemporalBlockCleanupService>();

        services.AddHttpClient<IGeoLocationService, GeoLocationService>(); ;
        services.AddSingleton<IBlockedAttemptLogRepo, BlockedAttemptLogRepo>();
        // forward headers configuration for reverse proxy
        services.Configure<ForwardedHeadersOptions>(options => {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });



        // register other infra services here
        return services;
    }
}
