using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs
{
    public class ResetPasswordDTO
    {
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
