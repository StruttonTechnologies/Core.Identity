using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Application.Authentication.Models;
using ST.Core.Identity.Domain.Authentication.Interfaces.Jwtoken;
using ST.Core.Identity.Domain.Authentication.Interfaces.Token;
using ST.Core.Identity.Infrastructure.Authentication.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.Authentication.JwtToken
{
    public static class TokenServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind JwtTokenOptions from appsettings.json or secrets.json
            services.Configure<JwtTokenOptions>(configuration.GetSection("JwtToken"));

            // Register the refresh token store (you can swap this with Redis, EF, etc.)
            //services.AddScoped<IRefreshTokenStore, InMemoryRefreshTokenStore>(); 

            // Register the JWT token manager
            services.AddScoped<IJwtUserTokenManager, JwtUserTokenManager>();

            return services;
        }
    }
}
