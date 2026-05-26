using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StruttonTechnologies.Core.Identity.API;
using StruttonTechnologies.Core.Identity.Coordinator;
using StruttonTechnologies.Core.Identity.EF;
using StruttonTechnologies.Core.Identity.JwtTokenManager;
using StruttonTechnologies.Core.Identity.Orchestration;

namespace StruttonTechnologies.Core.Identity.Composition
{
    /// <summary>
    /// Composition root for Core Identity services.
    /// Provides a single entry point for registering all identity-related services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all Core Identity services to the service collection.
        /// This is the primary method for registering the complete identity system.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing connection strings and JWT options.</param>
        /// <param name="dbOptionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentity<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<DbContextOptionsBuilder> dbOptionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>, new()
            where TRole : Domain.Entities.IdentityRole<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            // Add API layer (controllers)
            services.AddCoreIdentityApi();

            // Add Coordinator layer (MediatR handlers and coordinators)
            services.AddCoreIdentityCoordinator();

            // Add Orchestration layer (business logic)
            services.AddCoreIdentityOrchestration<TUser, TKey>(configuration);

            // Add Entity Framework layer (data access)
            services.AddCoreIdentityEntityFramework<TContext, TUser, TRole, TKey>(dbOptionsAction);

            // Add JWT Token Manager
            services.AddCoreIdentityJwtTokenManager<TKey>(configuration);

            return services;
        }

        /// <summary>
        /// Adds all Core Identity services with AccessTokenRevocation support (requires value type key).
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier (must be a value type).</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing connection strings and JWT options.</param>
        /// <param name="dbOptionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityWithRevocation<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<DbContextOptionsBuilder> dbOptionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>, new()
            where TRole : Domain.Entities.IdentityRole<TKey>, new()
            where TKey : struct, IEquatable<TKey>
        {
            // Add API layer (controllers)
            services.AddCoreIdentityApi();

            // Add Coordinator layer (MediatR handlers and coordinators)
            services.AddCoreIdentityCoordinator();

            // Add Orchestration layer (business logic)
            services.AddCoreIdentityOrchestration<TUser, TKey>(configuration);

            // Add Entity Framework layer with revocation support
            services.AddCoreIdentityEntityFrameworkWithRevocation<TContext, TUser, TRole, TKey>(dbOptionsAction);

            // Add JWT Token Manager
            services.AddCoreIdentityJwtTokenManager<TKey>(configuration);

            return services;
        }

        /// <summary>
        /// Adds all Core Identity services with default string key type.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing connection strings and JWT options.</param>
        /// <param name="dbOptionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentity<TContext, TUser, TRole>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<DbContextOptionsBuilder> dbOptionsAction)
            where TContext : CoreIdentityDbContext<string, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<string>, new()
            where TRole : Domain.Entities.IdentityRole<string>, new()
        {
            return services.AddCoreIdentity<TContext, TUser, TRole, string>(
                configuration,
                dbOptionsAction);
        }

        /// <summary>
        /// Adds all Core Identity services with SQL Server optimizations.
        /// Uses SqlServerRefreshTokenStore for better performance.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing connection strings and JWT options.</param>
        /// <param name="dbOptionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentitySqlServer<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<DbContextOptionsBuilder> dbOptionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>, new()
            where TRole : Domain.Entities.IdentityRole<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            // Add API layer (controllers)
            services.AddCoreIdentityApi();

            // Add Coordinator layer (MediatR handlers and coordinators)
            services.AddCoreIdentityCoordinator();

            // Add Orchestration layer (business logic)
            services.AddCoreIdentityOrchestration<TUser, TKey>(configuration);

            // Add Entity Framework layer with SQL Server optimizations
            services.AddCoreIdentityEntityFrameworkSqlServer<TContext, TUser, TRole, TKey>(dbOptionsAction);

            // Add JWT Token Manager
            services.AddCoreIdentityJwtTokenManager<TKey>(configuration);

            return services;
        }

        /// <summary>
        /// Adds all Core Identity services with SQL Server optimizations and AccessTokenRevocation support.
        /// Requires TKey to be a value type for AccessTokenRevocationStore.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier (must be a value type).</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing connection strings and JWT options.</param>
        /// <param name="dbOptionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentitySqlServerWithRevocation<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<DbContextOptionsBuilder> dbOptionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>, new()
            where TRole : Domain.Entities.IdentityRole<TKey>, new()
            where TKey : struct, IEquatable<TKey>
        {
            // Add API layer (controllers)
            services.AddCoreIdentityApi();

            // Add Coordinator layer (MediatR handlers and coordinators)
            services.AddCoreIdentityCoordinator();

            // Add Orchestration layer (business logic)
            services.AddCoreIdentityOrchestration<TUser, TKey>(configuration);

            // Add Entity Framework layer with SQL Server optimizations and revocation
            services.AddCoreIdentityEntityFrameworkSqlServerWithRevocation<TContext, TUser, TRole, TKey>(dbOptionsAction);

            // Add JWT Token Manager
            services.AddCoreIdentityJwtTokenManager<TKey>(configuration);

            return services;
        }

        /// <summary>
        /// Adds all Core Identity services with SQL Server optimizations and default string key type.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing connection strings and JWT options.</param>
        /// <param name="dbOptionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentitySqlServer<TContext, TUser, TRole>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<DbContextOptionsBuilder> dbOptionsAction)
            where TContext : CoreIdentityDbContext<string, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<string>, new()
            where TRole : Domain.Entities.IdentityRole<string>, new()
        {
            return services.AddCoreIdentitySqlServer<TContext, TUser, TRole, string>(
                configuration,
                dbOptionsAction);
        }
    }
}
