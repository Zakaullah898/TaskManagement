using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Configuration
{
    public class TaskAssignmentsConfig : IEntityTypeConfiguration<TaskAssignments>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TaskAssignments> builder)
        {
            builder.ToTable("TaskAssignments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TaskId).IsRequired();
            builder.Property(x => x.AssignedToUserId).IsRequired(false);
            builder.Property(x => x.AssignedByUserId).IsRequired(false);
            builder.Property(x => x.AssignedAt).IsRequired();
            builder.Property(x => x.isActive).IsRequired();

            // Relationships
            builder.HasOne(ta => ta.AssignedToUser)
                .WithMany(u => u.TaskAssignments)
                .HasForeignKey(ta => ta.AssignedToUserId);


        }
                
    }

}
