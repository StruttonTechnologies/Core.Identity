using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST.Core.Identity.Domain.Entities;
using ST.Core.Identity.Domain.Entities.Base;
using ST.Core.Identity.EF.Configuration;

namespace ST.Core.Identity.EF
{

    /// <summary>
    /// Generic IdentityDbContext base with shared configuration and entities.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key (e.g., Guid, int).</typeparam>
    /// <typeparam name="TUser">The user entity type.</typeparam>
    /// <typeparam name="TRole">The role entity type.</typeparam>
    public class CoreIdentityDbContext<TKey, TUser, TRole> : IdentityDbContext<TUser, TRole, TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
    {
        public CoreIdentityDbContext(DbContextOptions options) : base(options) { }

        public DbSet<RefreshToken<TKey>> RefreshTokens => Set<RefreshToken<TKey>>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            RefreshTokenConfiguration.Configure(builder.Entity<RefreshToken<TKey>>());
        }
    }
}