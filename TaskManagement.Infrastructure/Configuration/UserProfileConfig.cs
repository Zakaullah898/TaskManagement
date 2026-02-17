using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Configuration
{
    public class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.UserName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(350);
            builder.Property(p => p.JobTitle).IsRequired().HasMaxLength(250);
            builder.Property(p => p.DateJoined).IsRequired();
            builder.Property(p => p.ProfileImagePath).HasMaxLength(450);

            // One-to-One Relationship
            builder
                .HasOne(p => p.AppUser)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
