using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities;

public class UserRole
{
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? RoleDescription { get; set; }
    // Navigation property
    public virtual ICollection<AssignUserRole>? AssignRoles { get; set; }
}
