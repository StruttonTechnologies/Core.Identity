using StruttonTechnologies.Core.Identity.Domain.Entities;

namespace StruttonTechnologies.Core.Identity.EF.Configuration
{
    internal static class AccessTokenRevocationConfiguration
    {
        public static void Configure<TKey>(EntityTypeBuilder<AccessTokenRevocation<TKey>> builder)
            where TKey : IEquatable<TKey>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Jti).HasMaxLength(256).IsRequired();
            builder.HasIndex(x => x.Jti).IsUnique();
            builder.Property(x => x.ExpiresAtUtc).IsRequired();
        }
    }
}
