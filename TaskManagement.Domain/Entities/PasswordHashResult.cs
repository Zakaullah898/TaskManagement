using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class PasswordHashResult
    {
        public string? Hash { get; set; }
        public string? Salt { get; set; }
    }
}
