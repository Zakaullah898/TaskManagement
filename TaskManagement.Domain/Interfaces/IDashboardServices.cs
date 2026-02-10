using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IDashboardServices
    {
        Task<bool> DeletingUser(string id);
        Task<bool> UpdatingUser(AppUser model);
        Task<bool> UpdatingUserRole(string UserId, List<string> Role);
    }
}
