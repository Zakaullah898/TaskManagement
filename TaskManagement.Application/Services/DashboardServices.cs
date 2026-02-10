using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class DashboardServices : IDashboardServices
    {
        private readonly ITaskManagementRepo<AppUser> _appUserRepo;
        private readonly IAssignUserRoleRepo _assignUserRoleRepo;
        private readonly ITaskManagementRepo<UserRole> _userRolesRepo;
        public DashboardServices(ITaskManagementRepo<UserRole> userRolesRepo, ITaskManagementRepo<AppUser> appUserRepo, IAssignUserRoleRepo assignUserRoleRepo)
        {
            _appUserRepo = appUserRepo;
            _assignUserRoleRepo = assignUserRoleRepo;
            _userRolesRepo = userRolesRepo;
        }
        public async Task<bool> DeletingUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                new ArgumentNullException(nameof(id));
            }
            var user = await _appUserRepo.GetAsync(u => u.Id == id);
            if (user == null)
                {
                    new KeyNotFoundException("This user is not found");
            }
            var isDeleted = await _appUserRepo.DeleteAsync(u => u.Id == user!.Id);
            if (isDeleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdatingUser(AppUser model)
        {
            
            if (model == null)
            {
                new KeyNotFoundException("This user is not found");
            }
            var isUpdated = await _appUserRepo.UpdateAsync(model!);
            if (isUpdated) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdatingUserRole(string UserId, List<string> newRoles)
        {
            if (string.IsNullOrEmpty(UserId))
                throw new ArgumentNullException(nameof(UserId));
            if (newRoles == null)
                throw new ArgumentNullException(nameof(newRoles));
            var existingRoles = await _assignUserRoleRepo.GettingRoleIds(r => r.UserId == UserId);
            List<string> existingRoleIds = new List<string>();
            foreach (var existingRole in existingRoles)
            {
                var role = await _userRolesRepo.GetAsync(r => r.RoleId == existingRole, true);
                existingRoleIds.Add(role.RoleName!);
            }

            var rolesToAdd = newRoles.Except(existingRoleIds).ToList();
            Console.WriteLine($"Existing Roles: {string.Join(", ", existingRoleIds)}");
            foreach (var role in rolesToAdd)
            {
                Console.WriteLine($"Role to Add: {role}");
                if (!existingRoleIds.Contains(role))
                {
                    var assignedRole = await _userRolesRepo.GetAsync(r => r.RoleName == role, true);
                    var assignUserRole = new AssignUserRole
                    {
                        UserId = UserId,
                        RoleId = assignedRole.RoleId
                    };
                    var isUpdated = await _assignUserRoleRepo.UpdateAsync(assignUserRole);
                    if (isUpdated)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            var rolesToRemove = existingRoleIds.Except(newRoles).ToList();
            foreach (var role in rolesToRemove)
            {
                Console.WriteLine($"Role to Remove: {role}");
                if (!newRoles.Contains(role))
                {
                    var assignedRole = await _userRolesRepo.GetAsync(r => r.RoleName == role, true);
                    var isDeleted = await _assignUserRoleRepo.DeleteAsync(r => r.UserId == UserId && r.RoleId == assignedRole.RoleId);
                    if (isDeleted)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
                return true;
        }

}
    
}
