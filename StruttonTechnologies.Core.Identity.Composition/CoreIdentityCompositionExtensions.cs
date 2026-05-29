using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StruttonTechnologies.Core.Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;
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
    public static class CoreIdentityCompositionExtensions
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
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(dbOptionsAction);

            services.AddCoreIdentityApi();
            services.AddCoreAspNetIdentity<TContext, TUser, TRole, TKey>();
            services.AddCoreJwtAuthentication(configuration);
            services.AddCoreIdentityCoordinator<TUser, TKey>();
            services.AddCoreIdentityOrchestration<TUser, TKey>(configuration);
            services.AddCoreIdentityEntityFramework<TContext, TUser, TRole, TKey>(dbOptionsAction);
            services.AddCoreIdentityJwtTokenManager<TKey>(configuration);

            return services;
        }

        /// <summary>
        /// Adds all Core Identity services with AccessTokenRevocation support.
        /// This overload is retained for callers that explicitly choose the revocation-named API.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
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
            where TKey : IEquatable<TKey>
        {
            return services.AddCoreIdentity<TContext, TUser, TRole, TKey>(configuration, dbOptionsAction);
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
        /// Adds all Core Identity services with SQL Server optimized refresh token storage.
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
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(dbOptionsAction);

            services.AddCoreIdentityApi();
            services.AddCoreAspNetIdentity<TContext, TUser, TRole, TKey>();
            services.AddCoreJwtAuthentication(configuration);
            services.AddCoreIdentityCoordinator<TUser, TKey>();
            services.AddCoreIdentityOrchestration<TUser, TKey>(configuration);
            services.AddCoreIdentityEntityFrameworkSqlServer<TContext, TUser, TRole, TKey>(dbOptionsAction);
            services.AddCoreIdentityJwtTokenManager<TKey>(configuration);

            return services;
        }

        /// <summary>
        /// Adds all Core Identity services with SQL Server optimized refresh token storage and AccessTokenRevocation support.
        /// This overload is retained for callers that explicitly choose the revocation-named API.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type derived from CoreIdentityDbContext.</typeparam>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TRole">The type of role entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
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
            where TKey : IEquatable<TKey>
        {
            return services.AddCoreIdentitySqlServer<TContext, TUser, TRole, TKey>(configuration, dbOptionsAction);
        }

        /// <summary>
        /// Adds all Core Identity services with SQL Server optimized refresh token storage and default string key type.
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


        private static IServiceCollection AddCoreJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            JwtTokenOptions tokenOptions = configuration.GetSection("JwtTokenOptions").Get<JwtTokenOptions>()
                ?? new JwtTokenOptions();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = !string.IsNullOrWhiteSpace(tokenOptions.Issuer),
                        ValidateAudience = !string.IsNullOrWhiteSpace(tokenOptions.Audience),
                        ValidateIssuerSigningKey = !string.IsNullOrWhiteSpace(tokenOptions.SigningKey),
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        IssuerSigningKey = string.IsNullOrWhiteSpace(tokenOptions.SigningKey)
                            ? null
                            : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SigningKey)),
                    };
                });

            services.AddAuthorization();

            return services;
        }
        private static IServiceCollection AddCoreAspNetIdentity<TContext, TUser, TRole, TKey>(this IServiceCollection services)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>, new()
            where TRole : Domain.Entities.IdentityRole<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            services
                .AddIdentityCore<TUser>()
                .AddRoles<TRole>()
                .AddEntityFrameworkStores<TContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
