using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Application.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.API
{
    public static class IdentityProgramBuilderExtensions
    {
        public static IServiceCollection AddIdentityProgramServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Bind config options
            //services.Configure<JwtTokenOptions>(config.GetSection("JwtToken"));
            //services.Configure<TokenProviderOptionsConfig>(config.GetSection("Identity:TokenProvider"));

            //// Compose infrastructure
            //services.AddCoreIdentityServices(config);
            //services.AddSqlServerIdentity<ApplicationUser, ApplicationPerson, ApplicationDbContext>(config);

            //// Optional: Add your own API-layer services (e.g., email sender, audit logging)
            //services.AddScoped<IEmailSender, SmtpEmailSender>();

            return services;
        }
    }
}
