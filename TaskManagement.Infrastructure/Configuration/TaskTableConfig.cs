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
    internal class TaskTableConfig : IEntityTypeConfiguration<TaskTable>
    {
        public void Configure(EntityTypeBuilder<TaskTable> builder)
        { 
            builder.ToTable("TaskTable");
            builder.HasKey(x => x.TaskId);
            builder.Property(x => x.TaskId).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.priority).HasMaxLength(100);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(50);
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();
            builder.Property(x => x.CreatedByUserId).HasMaxLength(100);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired().ValueGeneratedNever().HasDefaultValue(false);



        }
    }
}
