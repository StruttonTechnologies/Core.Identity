using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.Token
{
    public static class TokenServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add token services and configurations here
            return services;
        }
    }
}
