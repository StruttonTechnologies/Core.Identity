using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ST.Core.Identity.Domain.Authentication.Entities;
using ST.Core.Identity.Infrastructure.EF.SqlServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.EF.SqlServer
{
    /// <summary>
    /// Provides extension methods for configuring SQL Server-based ASP.NET Core Identity services.
    /// </summary>
    //public static class SqlServerIdentityBuilderExtensions
    //{
    //    /// <summary>
    //    /// Adds and configures ASP.NET Core Identity with SQL Server as the backing store.
    //    /// </summary>
    //    /// <typeparam name="TUser">The user entity type, derived from <see cref="IdentityUserBase{TPerson}"/>.</typeparam>
    //    /// <typeparam name="TPerson">The person entity type.</typeparam>
    //    /// <typeparam name="TContext">The database context type, derived from <see cref="IdentityDbContextBase{TUser, TPerson}"/>.</typeparam>
    //    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    //    /// <param name="config">The application configuration containing the connection string.</param>
    //    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    //    public static IServiceCollection AddSqlServerIdentity<TUser, TPerson, TContext>(this IServiceCollection services,IConfiguration config)
    //        where TUser : IdentityUserBase<TPerson>
    //        where TPerson : class
    //        where TContext : IdentityDbContextBase<TUser, TPerson>
    //    {
    //        services.AddDbContext<TContext>(options =>
    //            options.UseSqlServer(config.GetConnectionString("IdentityConnection")));

    //        services.AddIdentityCore<TUser>()
    //            .AddRoles<IdentityRole<Guid>>()
    //            .AddEntityFrameworkStores<TContext>()
    //            .AddTokenProvider<DataProtectorTokenProvider<TUser>>(TokenOptions.DefaultProvider);

    //        services.AddSqlServerIdentityRepositories<TUser, TPerson, TContext>();

    //        return services;
    //    }
    //}
}
