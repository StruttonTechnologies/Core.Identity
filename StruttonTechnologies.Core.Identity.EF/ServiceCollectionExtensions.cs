using Microsoft.Extensions.DependencyInjection;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.EF.Repositories;

namespace StruttonTechnologies.Core.Identity.EF
{
    /// <summary>
    /// Extension methods for configuring Core Identity Entity Framework services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Core Identity Entity Framework services to the service collection.
        /// Registers DbContext and token storage repositories.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="optionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityEntityFramework<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>
            where TRole : Domain.Entities.IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            // Register DbContext
            services.AddDbContext<TContext>(optionsAction);

            // Register token stores
            services.AddScoped<IRefreshTokenStore<TKey>, EfRefreshTokenStore<TKey>>();

            return services;
        }

        /// <summary>
        /// Adds Core Identity Entity Framework services with value type key support for AccessTokenRevocationStore.
        /// This overload supports AccessTokenRevocation which requires a struct key type.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier (must be a value type).</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="optionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityEntityFrameworkWithRevocation<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>
            where TRole : Domain.Entities.IdentityRole<TKey>
            where TKey : struct, IEquatable<TKey>
        {
            // Register DbContext
            services.AddDbContext<TContext>(optionsAction);

            // Register token stores
            services.AddScoped<IRefreshTokenStore<TKey>, EfRefreshTokenStore<TKey>>();
            services.AddScoped<IAccessTokenRevocationStore<TKey>, EfAccessTokenRevocationStore<TKey>>();

            return services;
        }

        /// <summary>
        /// Adds Core Identity Entity Framework services with default string key type.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="optionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityEntityFramework<TContext, TUser, TRole>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : CoreIdentityDbContext<string, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<string>
            where TRole : Domain.Entities.IdentityRole<string>
        {
            return services.AddCoreIdentityEntityFramework<TContext, TUser, TRole, string>(optionsAction);
        }

        /// <summary>
        /// Adds Core Identity Entity Framework services with SQL Server optimizations.
        /// Uses SqlServerRefreshTokenStore for better performance on SQL Server.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="optionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityEntityFrameworkSqlServer<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>
            where TRole : Domain.Entities.IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            // Register DbContext
            services.AddDbContext<TContext>(optionsAction);

            // Register SQL Server optimized token stores
            services.AddScoped<IRefreshTokenStore<TKey>, SqlServerRefreshTokenStore<TKey>>();

            return services;
        }

        /// <summary>
        /// Adds Core Identity Entity Framework services with SQL Server optimizations and AccessTokenRevocation support.
        /// Requires TKey to be a value type for AccessTokenRevocationStore.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier (must be a value type).</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="optionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityEntityFrameworkSqlServerWithRevocation<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>
            where TRole : Domain.Entities.IdentityRole<TKey>
            where TKey : struct, IEquatable<TKey>
        {
            // Register DbContext
            services.AddDbContext<TContext>(optionsAction);

            // Register SQL Server optimized token stores
            services.AddScoped<IRefreshTokenStore<TKey>, SqlServerRefreshTokenStore<TKey>>();
            services.AddScoped<IAccessTokenRevocationStore<TKey>, EfAccessTokenRevocationStore<TKey>>();

            return services;
        }
    }
}
