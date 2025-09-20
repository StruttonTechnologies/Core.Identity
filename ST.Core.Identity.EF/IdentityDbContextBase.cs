using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Identity.EF.Configuration;
using ST.Core.Identity.Infrastructure.EF.Configuration;

namespace ST.Core.Identity.EF
{
    /// <summary>
    /// Base identity DbContext with shared configuration for all providers.
    /// </summary>
    public abstract class IdentityDbContextBase<TUser, TPerson> :
        IdentityDbContext<TUser, IdentityRole<Guid>, Guid>
         where TUser : IdentityUserBase<TPerson>
         where TPerson : class
    {
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected IdentityDbContextBase(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            RefreshTokenConfiguration.Configure(builder.Entity<RefreshToken>());
            IdentityUserConfiguration.Configure<TUser, TPerson>(builder.Entity<TUser>());
        }
    }
}