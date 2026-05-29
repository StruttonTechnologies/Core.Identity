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
            ArgumentNullException.ThrowIfNull(optionsAction);

            services.AddDbContext<TContext>(optionsAction);

            services.AddScoped<IRefreshTokenStore<TKey>, EfRefreshTokenStore<TContext, TUser, TRole, TKey>>();
            services.AddScoped<IAccessTokenRevocationStore<TKey>, EfAccessTokenRevocationStore<TContext, TUser, TRole, TKey>>();

            return services;
        }

        /// <summary>
        /// Adds Core Identity Entity Framework services with access token revocation support.
        /// This overload is retained for callers that explicitly choose the revocation-named API.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="optionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityEntityFrameworkWithRevocation<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>
            where TRole : Domain.Entities.IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            return services.AddCoreIdentityEntityFramework<TContext, TUser, TRole, TKey>(optionsAction);
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
        /// Adds Core Identity Entity Framework services with SQL Server optimized refresh token storage.
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
            ArgumentNullException.ThrowIfNull(optionsAction);

            services.AddDbContext<TContext>(optionsAction);

            services.AddScoped<IRefreshTokenStore<TKey>, SqlServerRefreshTokenStore<TContext, TUser, TRole, TKey>>();
            services.AddScoped<IAccessTokenRevocationStore<TKey>, EfAccessTokenRevocationStore<TContext, TUser, TRole, TKey>>();

            return services;
        }

        /// <summary>
        /// Adds Core Identity Entity Framework services with SQL Server optimized refresh token storage and access token revocation support.
        /// This overload is retained for callers that explicitly choose the revocation-named API.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="optionsAction">Action to configure the DbContext options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityEntityFrameworkSqlServerWithRevocation<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>
            where TRole : Domain.Entities.IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            return services.AddCoreIdentityEntityFrameworkSqlServer<TContext, TUser, TRole, TKey>(optionsAction);
        }
    }
}
