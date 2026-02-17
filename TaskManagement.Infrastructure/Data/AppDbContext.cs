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
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<TaskTable> TaskTable { get; set; }
        public virtual DbSet<TaskAssignments> TaskAssignments { get; set; }
        public virtual DbSet<AssignUserRole> AssignUserRoles { get; set; }
        public virtual DbSet<OTP> OTPs { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.ApplyConfiguration(new AppUserConfig());
            modelBuilder.ApplyConfiguration(new UserRoleConfig());
            modelBuilder.ApplyConfiguration(new TaskTableConfig());
            modelBuilder.ApplyConfiguration(new TaskAssignmentsConfig());
            modelBuilder.ApplyConfiguration(new AssignRoleConfig());
            modelBuilder.ApplyConfiguration(new UserProfileConfig());
        }

        }
}
