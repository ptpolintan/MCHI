using MHCI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHCI.Infrastructure.Persistence.CheckIns
{
    public class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
    {
        public void Configure(EntityTypeBuilder<CheckIn> builder)
        {
            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.UserId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Mood)
                .IsRequired();

            builder.Property(c => c.Notes)
                .HasMaxLength(500);

            // DateOnly mapping
            builder.Property(c => c.CreatedAt)
                .HasColumnType("date")
                .IsRequired();

            // Index to enforce one check-in per day per user
            builder.HasIndex(c => new { c.UserId, c.CreatedAt })
                   .IsUnique();
        }
    }
}
