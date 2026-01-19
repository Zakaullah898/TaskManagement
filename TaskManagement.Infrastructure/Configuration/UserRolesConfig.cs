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
    internal class UserRolesConfig : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasKey(x => x.RoleId);
            builder.Property(x => x.RoleId).UseIdentityColumn();
            builder.Property(x => x.RoleName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.RoleDescription).HasMaxLength(250);
        }
        }
}
