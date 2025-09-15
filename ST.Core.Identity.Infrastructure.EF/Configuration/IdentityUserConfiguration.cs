using ST.Core.Identity.Domain.Authentication.Entities;
using System.ComponentModel.DataAnnotations;

namespace ST.Core.Identity.Infrastructure.EF.Configuration
{
    public static class IdentityUserConfiguration
    {
        public static void Configure<TUser, TPerson>(EntityTypeBuilder<TUser> entity)
            where TUser : IdentityUserBase<TPerson>
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



