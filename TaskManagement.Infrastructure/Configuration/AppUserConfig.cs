using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Infrastructure.Configuration
{
    internal class AppUserConfig :IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.Salt).IsRequired();
            builder.Property(x => x.DateRegistered).IsRequired();
            builder.Property(x => x.LastLogin);
            builder.Property(x => x.IsActive).IsRequired();

            // Adding foreign key relationship with TaskTable
            builder.HasMany(u => u.TaskAssignments)
                .WithOne(ta => ta.AssignedToUser)
                .HasForeignKey(ta => ta.AssignedToUserId);
        }
    }
}
