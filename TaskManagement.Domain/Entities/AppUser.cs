using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class AppUser
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime LastLogin { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
        public bool isLogin { get; set; } = false;
        public bool IsActive { get; set; }

        public virtual ICollection<TaskAssignments>? TaskAssignments { get; set; }

    }
}
