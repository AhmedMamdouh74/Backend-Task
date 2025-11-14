using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DI
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<ICountryBlockService, CountryBlockService>();
            services.AddScoped<IIpService, IpService>();
        }
    }
}
