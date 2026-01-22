using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Configuration
{
    public class AssignRoleConfig : IEntityTypeConfiguration<AssignUserRole>
    {
        public void Configure(EntityTypeBuilder<AssignUserRole> builder)
        {
            builder.ToTable("AssignRoles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();
            // Define foreign key relationship with AppUser
            builder.HasOne(ar => ar.User)
                   .WithMany(u => u.AssignUserRoles)
                   .HasForeignKey(ar => ar.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Define foreign key relationship with UserRoles
            builder.HasOne(ar => ar.Role)
                   .WithMany(r => r.AssignUserRoles)
                   .HasForeignKey(ar => ar.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
