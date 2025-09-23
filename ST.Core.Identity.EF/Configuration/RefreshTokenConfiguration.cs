using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ST.Core.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Infrastructure.EF.Configuration
{
    public static class RefreshTokenConfiguration
    {
        public static void Configure<TKey>(EntityTypeBuilder<RefreshToken<TKey>> entity)
            where TKey : IEquatable<TKey>
        {
            entity.ToTable("RefreshTokens");
            entity.HasKey(e => e.Token);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.ExpiresAt).IsRequired();
            entity.Property(e => e.IsRevoked).IsRequired();
            entity.Property(e => e.RevokedAt);
        }
    }
}
