using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using StruttonTechnologies.Core.Identity.API;
using StruttonTechnologies.Core.Identity.Coordinator;
using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.EF;
using StruttonTechnologies.Core.Identity.JwtTokenManager;
using StruttonTechnologies.Core.Identity.Orchestration;

namespace StruttonTechnologies.Core.Identity.Composition
{
    public static class CoreIdentityCompositionExtensions
    {
        public static IServiceCollection AddCoreIdentity<TContext, TUser, TRole, TKey>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : CoreIdentityDbContext<TKey, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<TKey>, new()
            where TRole : Domain.Entities.IdentityRole<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddCoreIdentityApi();
            services.AddCoreAspNetIdentity<TContext, TUser, TRole, TKey>();
            services.AddCoreJwtAuthentication(configuration);
            services.AddCoreIdentityCoordinator<TUser, TKey>();
            services.AddCoreIdentityOrchestration<TUser, TKey>(configuration);
            services.AddCoreIdentityEntityFramework<TContext, TUser, TRole, TKey>();
            services.AddCoreIdentityJwtTokenManager<TKey>(configuration);

            return services;
        }

        public static IServiceCollection AddCoreIdentity<TContext, TUser, TRole>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : CoreIdentityDbContext<string, TUser, TRole>
            where TUser : Domain.Entities.IdentityUser<string>, new()
            where TRole : Domain.Entities.IdentityRole<string>, new()
        {
            return services.AddCoreIdentity<TContext, TUser, TRole, string>(
                configuration);
        }

        private static IServiceCollection AddCoreJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            JwtTokenOptions tokenOptions =
                configuration.GetSection("JwtTokenOptions").Get<JwtTokenOptions>()
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
                            : new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(tokenOptions.SigningKey)),
                    };
                });

            services.AddAuthorization();

            return services;
        }

        private static IServiceCollection AddCoreAspNetIdentity<TContext, TUser, TRole, TKey>(
            this IServiceCollection services)
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
