using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Domain.Authentication.Interfaces.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.UserManager
{
    public static class UserManagerServiceCollectionExtensions
    {
        public static IServiceCollection AddUserManagerServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IUserLookupService, UserLookupService>();
            //services.AddScoped<IPasswordService, PasswordService>();
            // Add other UserManager-related services

            return services;
        }
    }
}


