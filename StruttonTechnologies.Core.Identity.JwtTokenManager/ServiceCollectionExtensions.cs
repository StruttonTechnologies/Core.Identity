using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StruttonTechnologies.Core.Identity.Domain.Contracts.JwtToken;
using StruttonTechnologies.Core.Identity.Domain.Models;

namespace StruttonTechnologies.Core.Identity.JwtTokenManager
{
    /// <summary>
    /// Extension methods for configuring JWT Token Manager services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds JWT Token Manager services to the service collection.
        /// Registers the JWT token provider and configuration.
        /// </summary>
        /// <typeparam name="TKey">The type of the user identifier.</typeparam>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing JWT token options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityJwtTokenManager<TKey>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TKey : IEquatable<TKey>
        {
            ArgumentNullException.ThrowIfNull(configuration);

            // Configure JWT token options
            services.Configure<JwtTokenOptions>(configuration.GetSection("JwtTokenOptions"));

            // Create and register JwtTokenOptions as a singleton for direct injection
            JwtTokenOptions? tokenOptions = configuration.GetSection("JwtTokenOptions").Get<JwtTokenOptions>();
            if (tokenOptions != null)
            {
                services.AddSingleton(tokenOptions);
            }

            // Register JWT user token manager
            services.AddScoped<IJwtUserTokenManager<TKey>, JwtUserTokenManager<TKey>>();

            return services;
        }

        /// <summary>
        /// Adds JWT Token Manager services with default string key type.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration containing JWT token options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityJwtTokenManager(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddCoreIdentityJwtTokenManager<string>(configuration);
        }
    }
}
