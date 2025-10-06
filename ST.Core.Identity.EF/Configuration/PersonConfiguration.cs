using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ST.Core.Identity.Domain.Entities;

namespace ST.Core.Identity.EF.Configuration
{
    public static class PersonConfiguration
    {
        public static void Configure<TPerson, TKey>(EntityTypeBuilder<TPerson> entity)
            where TPerson : PersonBase<TPerson, TKey>
            where TKey : IEquatable<TKey>
        {
            entity.ToTable("Persons");

            entity
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.ModifiedBy)
                .WithMany()
                .HasForeignKey(p => p.ModifiedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}