using Microsoft.EntityFrameworkCore;
using ST.Core.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.EF.Configuration
{
    public static class PersonConfiguration
    {
        public static void Configure<TPerson>(EntityTypeBuilder<TPerson> entity)
            where TPerson : PersonBase<TPerson>
        {
            entity.ToTable("Persons");

            entity.Property(p => p.FirstName).HasMaxLength(256);
            entity.Property(p => p.LastName).HasMaxLength(256);
            entity.Property(p => p.ContactEmail).HasMaxLength(256);

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
