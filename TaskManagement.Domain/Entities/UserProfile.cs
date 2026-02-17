using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? JobTitle { get; set; }
        public DateTime DateJoined { get; set; }
        public string? ProfileImagePath { get; set; }  // ✅ Store this in DB
        
    }
}
