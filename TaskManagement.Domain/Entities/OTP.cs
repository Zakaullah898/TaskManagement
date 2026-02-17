using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class OTP
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
        public string? OtpHash { get; set; }
        public bool IsUsed { get; set; }
        public string? Email { get; set; }
        public DateTime ExpiryTime { get; set; }
        public int AttemptCount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
