using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class AssignUserRole
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public AppUser User { get; set; } =null!;

        public int RoleId { get; set; }
        public UserRole Role { get; set; } = null!;

    }
}
