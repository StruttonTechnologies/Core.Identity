using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST.Core.Identity.Domain.Authentication.Entities;
using ST.Core.Identity.Infrastructure.EF.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.EF
{
    //public abstract class IdentityDbContextBase<TUser, TPerson>
    //: IdentityDbContext<TUser, IdentityRole<Guid>, Guid>
    //where TUser : IdentityUserBase<TPerson>
    //where TPerson : class
    //{
    //    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    //    protected IdentityDbContextBase(DbContextOptions options) : base(options) { }

    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {
    //        base.OnModelCreating(builder);

    //        RefreshTokenConfiguration.Configure(builder.Entity<RefreshToken>());
    //        IdentityUserConfiguration.Configure<TUser, TPerson>(builder.Entity<TUser>());
    //    }
    //}
}
