using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
        
    }
}
