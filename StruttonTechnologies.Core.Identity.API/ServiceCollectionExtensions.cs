using Microsoft.Extensions.DependencyInjection;

namespace StruttonTechnologies.Core.Identity.API
{
    /// <summary>
    /// Extension methods for configuring Core Identity API services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Core Identity API controllers and services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddCoreIdentityApi(this IServiceCollection services)
        {
            // Register controllers (if using AddControllersWithViews or AddControllers at app level, this is automatic)
            // The API project provides controllers but doesn't register additional services
            // Controllers are discovered and registered by the hosting application
            return services;
        }
    }
}
