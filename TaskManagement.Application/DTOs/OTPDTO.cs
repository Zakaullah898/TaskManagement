using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs
{
    public class OTPDTO
    {
        public string? Email { get; set; }
        public string? Otp { get; set; }
        public DateTime ExpiryTime { get; set; } = DateTime.Now;
    }
}
