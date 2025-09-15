using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Application.Authentication.Models;
using ST.Core.Identity.Domain.Authentication.Entities;
using ST.Core.Identity.Domain.Authentication.Interfaces.Jwtoken;
using ST.Core.Identity.Domain.Authentication.Interfaces.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.JwtToken
{
    public static class JwtTokenBuilderExtensions
    {
        public static IServiceCollection AddJwtTokenConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtTokenOptions>(config.GetSection("JwtToken"));
            return services;
        }

        public static IServiceCollection AddJwtTokenServices<TUser, TPerson>(this IServiceCollection services)
            where TUser : IdentityUserBase<TPerson>
            where TPerson : class
        {
           // services.AddScoped<IJwtUserTokenManager, JwtUserTokenManager<TUser, TPerson>>();
            return services;
        }

    }
}
