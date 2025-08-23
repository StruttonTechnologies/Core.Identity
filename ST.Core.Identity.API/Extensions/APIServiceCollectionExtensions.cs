using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.DI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API.Extensions
{
    public static class APIServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityServices(configuration);

            // Add other API-specific services and configurations here
            return services;
        }
    }
}
