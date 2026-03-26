using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using StruttonTechnologies.Core.Identity.Domain.Entities;
using StruttonTechnologies.Core.Identity.EF.Configuration;

namespace StruttonTechnologies.Core.Identity.EF
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
        where TRole : Microsoft.AspNetCore.Identity.IdentityRole<TKey>
    {
        public CoreIdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<RefreshToken<TKey>> RefreshTokens => Set<RefreshToken<TKey>>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            base.OnModelCreating(builder);

            RefreshTokenConfiguration.Configure(builder.Entity<RefreshToken<TKey>>());
        }
    }
}
