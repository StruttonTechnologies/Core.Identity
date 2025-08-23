using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Application.Extensions
{
    public static class IdentityApplicationServiceExtensions
    {
        public static IServiceCollection AddIdentityApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Add application services and configurations here
            return services;
        }
    }
}
