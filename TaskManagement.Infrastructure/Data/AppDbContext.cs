using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Configuration;

namespace TaskManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<TaskTable> TaskTable { get; set; }
        public virtual DbSet<TaskAssignments> TaskAssignments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.ApplyConfiguration(new AppUserConfig());
            modelBuilder.ApplyConfiguration(new UserRolesConfig());
            modelBuilder.ApplyConfiguration(new TaskTableConfig());
            modelBuilder.ApplyConfiguration(new TaskAssignmentsConfig());
        }

        }
}
