using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StruttonTechnologies.Core.Identity.Domain.Models;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Orchestration.Contracts.UserManager;
using StruttonTechnologies.Core.Identity.Orchestration.JwtTokens;
using StruttonTechnologies.Core.Identity.Orchestration.UserManager;

namespace StruttonTechnologies.Core.Identity.Orchestration
{
    /// <summary>
    /// Extension methods for configuring Core Identity Orchestration services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Core Identity Orchestration services to the service collection.
        /// Registers authentication and token orchestration services.
        /// </summary>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing JWT token options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityOrchestration<TUser, TKey>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TUser : IdentityUser<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            ArgumentNullException.ThrowIfNull(configuration);

            // Configure JWT token options
            services.Configure<JwtTokenOptions>(configuration.GetSection("JwtTokenOptions"));

            // Register orchestration services
            services.AddScoped<IAuthenticationOrchestration<TKey>, AuthenticationOrchestration<TUser, TKey>>();
            services.AddScoped<ITokenOrchestration<TKey>, TokenOrchestration<TKey>>();

            return services;
        }

        /// <summary>
        /// Adds Core Identity Orchestration services with default string key type.
        /// </summary>
        /// <typeparam name="TUser">The type of user entity.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing JWT token options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityOrchestration<TUser>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TUser : IdentityUser<string>, new()
        {
            return services.AddCoreIdentityOrchestration<TUser, string>(configuration);
        }
    }
}
