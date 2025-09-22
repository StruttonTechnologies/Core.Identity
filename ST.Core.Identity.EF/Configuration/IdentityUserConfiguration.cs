using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ST.Core.Identity.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace ST.Core.Identity.EF.Configuration
{
    public static class IdentityUserConfiguration
    {
        public static void Configure<TKey,TUser, TPerson>(EntityTypeBuilder<TUser> entity)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUserBase<TKey,TPerson>
            where TPerson : class
        {
            entity.ToTable("Users");
            entity.Property(u => u.CreateDate).IsRequired();
            entity.Property(u => u.ProviderName).HasMaxLength(128);
            entity.Property(u => u.IsActive).IsRequired();

            entity.HasOne(u => u.Person)
                  .WithMany()
                  .HasForeignKey(u => u.PersonId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}



