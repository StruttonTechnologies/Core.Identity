using ST.Core.Identity.Application.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Infrastructure;

namespace ST.Core.Identity.DI.Extensions
{
    public static class IdentityICollectionServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityInfrastructureServices(configuration);
            services.AddIdentityApplicationServices(configuration);
            return services;
        }
    }
}
