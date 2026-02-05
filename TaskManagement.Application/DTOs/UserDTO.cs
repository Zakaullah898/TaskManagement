using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<String>? Role { get; set; } = new List<string>();
        public bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }

    }
}
