using Microsoft.Extensions.DependencyInjection;

using StruttonTechnologies.Core.Identity.API.Controllers;

namespace StruttonTechnologies.Core.Identity.API
{
    /// <summary>
    /// Extension methods for configuring Core Identity API services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Core Identity API controllers to the service collection and exposes this assembly as an MVC application part.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityApi(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddApplicationPart(typeof(AuthenticationController).Assembly);

            return services;
        }
    }
}
