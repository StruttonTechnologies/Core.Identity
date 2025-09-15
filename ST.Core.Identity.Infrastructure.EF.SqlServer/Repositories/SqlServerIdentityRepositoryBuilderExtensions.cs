using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Domain.Authentication.Entities;
using ST.Core.Identity.Domain.Authentication.Interfaces.Jwtoken;
using ST.Core.Identity.Infrastructure.EF.SqlServer.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.EF.SqlServer.Repositories
{
    public static class SqlServerIdentityRepositoryBuilderExtensions
    {
        public static IServiceCollection AddSqlServerIdentityRepositories<TUser, TPerson, TContext>(
            this IServiceCollection services)
            where TUser : IdentityUserBase<TPerson>
            where TPerson : class
            //where TContext : IdentityDbContextBase<TUser, TPerson>
        {
            //services.AddScoped<IRefreshTokenStore, SqlServerRefreshTokenStore<TUser, TPerson>>();
            // Add other repository registrations here

            return services;
        }
    }
}
