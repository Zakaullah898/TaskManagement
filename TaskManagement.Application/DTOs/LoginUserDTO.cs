using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs
{
    public class LoginUserDTO
    {
        public string? EnteredPassword { get; set; }
        public string? Email { get; set; }
    }
}
