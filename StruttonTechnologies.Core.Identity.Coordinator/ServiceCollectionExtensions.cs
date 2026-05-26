using Microsoft.Extensions.DependencyInjection;

using StruttonTechnologies.Core.Identity.Coordinator.Authentication.Coordinator;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Authentication;
using StruttonTechnologies.Core.Identity.Coordinator.Contracts.Users;
using StruttonTechnologies.Core.Identity.Coordinator.Users.Coordinator;

namespace StruttonTechnologies.Core.Identity.Coordinator
{
    /// <summary>
    /// Extension methods for configuring Core Identity Coordinator services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Core Identity Coordinator services to the service collection.
        /// Registers all MediatR handlers for identity commands and queries, plus coordinator services.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityCoordinator(this IServiceCollection services)
        {
            // Register MediatR handlers from this assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

            // Register coordinator services
            services.AddScoped<IAuthenticationCoordinator, AuthenticationCoordinator>();
            services.AddScoped<IUserCoordinator, UserCoordinator>();

            return services;
        }
    }
}
