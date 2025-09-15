using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Application.Extensions;
using ST.Core.Identity.Infrastructure;
using ST.Core.Identity.Infrastructure.Configuration;

namespace ST.Core.Identity.DI.Extensions
{
    /// <summary>
    /// Extension methods for registering identity-related services in the <see cref="IServiceCollection"/>.
    /// </summary>
    public static class IdentityICollectionServiceExtensions
    {
        /// <summary>
        /// Adds identity infrastructure and application services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddIdentityInfrastructureServices(configuration);
            //services.AddIdentityApplicationServices(configuration);
            //services.AddEntityFrameWorkServices(configuration);

            return services;
        }

        //private static IServiceCollection AddEntityFrameWorkServices<TUser>(IServiceCollection services,IConfiguration configuration, IdentityDatabaseProvider provider)
        //    where TUser : IdentityUser
        //{
        //    return provider switch
        //    {
        //        IdentityDatabaseProvider.SqlServer =>
        //            services.AddIdentitySqlServerServices<TUser>(configuration),

        //        IdentityDatabaseProvider.PostgreSql =>
        //            services.AddIdentityPostgreSqlServices<TUser>(configuration),

        //        IdentityDatabaseProvider.MySql =>
        //            services.AddIdentityMySqlServices<TUser>(configuration),

        //        IdentityDatabaseProvider.Sqlite =>
        //            services.AddIdentitySqliteServices<TUser>(configuration),

        //        _ => throw new NotSupportedException($"Unsupported provider: {provider}")
        //    };
        //}


    }
}
