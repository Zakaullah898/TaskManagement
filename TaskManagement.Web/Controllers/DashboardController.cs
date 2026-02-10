using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Utilities;
using TaskManagement.Web.Models;

namespace TaskManagement.Web.Controllers
{
    // endpoint for creating tasks
    [Authorize(Roles = "Manager")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private readonly IDashboardServices _dashboardServices;
        private readonly ITaskManagementRepo<UserRole> _userRolesRepo;
        private readonly IAssignUserRoleRepo _assignUserRoleRepo;
        private readonly ITaskManagementRepo<TaskAssignments> _taskManagementRepo;
        private readonly ITaskManagementRepo<AppUser> _appUserRepo;
        private readonly IHelperMethods _helperMethods;

        public DashboardController(
            IHelperMethods helperMethods,
            ILogger<DashboardController> logger,
            ITaskService taskService,
            IMapper mapper,
            IAuthService authService,
            ITaskManagementRepo<UserRole> userRolesRepo,
            IAssignUserRoleRepo assignUserRoleRepo,
            ITaskManagementRepo<TaskAssignments> taskManagementRepo,
            IDashboardServices dashboardServices,
            ITaskManagementRepo<AppUser> appUserRepo
            )
        {
            _logger = logger;
            _taskService = taskService;
            _mapper = mapper;
            _authService = authService;
            _userRolesRepo = userRolesRepo;
            _assignUserRoleRepo = assignUserRoleRepo;
            _taskManagementRepo = taskManagementRepo;
            _dashboardServices = dashboardServices;
            _helperMethods = helperMethods;
            _appUserRepo = appUserRepo;
        }
        public IActionResult AdminDashboard()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasks();


            var taskDTOs = _mapper.Map<IEnumerable<TaskTableDTO>>(tasks);

            return PartialView("_AllTaskTablePartial", taskDTOs);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsers();

            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            foreach (var user in userDTOs)
            {
                var assignedRoles = await _assignUserRoleRepo.GettingRoleIds(r => r.UserId == user.Id);
                if (assignedRoles != null)
                {
                    foreach (var assignedRole in assignedRoles)
                    {
                        _logger.LogInformation($"User: {user.UserName}, Assigned Role ID: {assignedRole}");
                        var role = await _userRolesRepo.GetAsync(r => r.RoleId == assignedRole, true);
                        user.Role!.Add(role.RoleName!);
                        _logger.LogInformation($"User: {user.UserName}, Assigned Role Name: {role.RoleName}");
                    }
                }




            }
            return PartialView("_AllUsersTablePartial", userDTOs);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var isDeleted = await _dashboardServices.DeletingUser(id);
                if (isDeleted)
                {
                    return Ok(new { message = "User deleted successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to delete user" });
                }

            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // endpoint for updating user information
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody]UserDTO model)
        {
            try
            {
                
            var user = await _appUserRepo.GetAsync(u => u.Id == model.Id, true);
                if (user != null) 
                {
                    user.UserName = model.UserName;
                    user.IsActive = model.IsActive;
                }
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var result = _helperMethods.HashPassword(model.Password);
                    user!.PasswordHash = result.Hash;
                    user.Salt = result.Salt;
                }
                var isAssignedRole = await _dashboardServices.UpdatingUserRole(model.Id!,model.Role!);
                if (!isAssignedRole)
                {
                    return BadRequest(new { message = $"Failed to updating assigne role" });
                }

                foreach (var role in model.Role!)
                {
                    var userRole = await _userRolesRepo.GetAsync(r => r.RoleName == role,true);
                    if (userRole != null)
                    {
                        var assignUserRole = new AssignUserRole
                        {
                            UserId = model.Id!,
                            RoleId = userRole.RoleId
                        };

                    }
                }
                var isUpdated = await _dashboardServices.UpdatingUser(user!);
                if (isUpdated)
                {
                    return Ok(new { message = "User updated successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to update user" });
                }
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
