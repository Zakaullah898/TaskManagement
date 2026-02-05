using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;

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
        private readonly ITaskManagementRepo<UserRole> _userRolesRepo;
        private readonly IAssignUserRoleRepo _assignUserRoleRepo;
        private readonly ITaskManagementRepo<TaskAssignments> _taskManagementRepo;

        public DashboardController(
            ILogger<DashboardController> logger,
            ITaskService taskService,
            IMapper mapper,
            IAuthService authService,
            ITaskManagementRepo<UserRole> userRolesRepo,
            IAssignUserRoleRepo assignUserRoleRepo,
            ITaskManagementRepo<TaskAssignments> taskManagementRepo
            )
        {
            _logger = logger;
            _taskService = taskService;
            _mapper = mapper;
            _authService = authService;
            _userRolesRepo = userRolesRepo;
            _assignUserRoleRepo = assignUserRoleRepo;
            _taskManagementRepo = taskManagementRepo;
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
                        var role = await _userRolesRepo.GetAsync(r => r.RoleId == assignedRole ,true);
                        user.Role!.Add(role.RoleName!);
                        _logger.LogInformation($"User: {user.UserName}, Assigned Role Name: {role.RoleName}");
                    }
                }

                

                 
            }
            return PartialView("_AllUsersTablePartial", userDTOs);
        }
        }


}
