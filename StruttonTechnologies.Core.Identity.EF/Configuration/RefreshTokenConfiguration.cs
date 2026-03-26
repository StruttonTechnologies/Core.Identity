using StruttonTechnologies.Core.Identity.Domain.Entities;

namespace StruttonTechnologies.Core.Identity.EF.Configuration
{
    public static class RefreshTokenConfiguration
    {
        public static void Configure<TKey>(EntityTypeBuilder<RefreshToken<TKey>> entity)
            where TKey : IEquatable<TKey>
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity.HasKey(e => e.Token); // Token is the PK

            entity.Property(e => e.Token)
                .HasMaxLength(512)
                .IsRequired();

            entity.Property(e => e.UserId).IsRequired();

            entity.Property(e => e.Username)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.ExpiresAt).IsRequired();
            entity.Property(e => e.IsRevoked).IsRequired();

            entity.Property(e => e.CreatedByIp).HasMaxLength(64);
            entity.Property(e => e.RevokedByIp).HasMaxLength(64);
            entity.Property(e => e.ReplacedByToken).HasMaxLength(512);
            entity.Property(e => e.ReasonRevoked).HasMaxLength(256);
        }
    }
}
