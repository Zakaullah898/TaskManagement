using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Web.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITaskService _taskService;
        private readonly ITaskManagementRepo<TaskAssignments> _taskManagementRepo;
        IMapper _mapper;
        public AnalyticsController(
            ITaskService taskService,
            IMapper mapper,
            IAuthService authService,
            ITaskManagementRepo<TaskAssignments> taskManagementRepo)
        {
            _taskService = taskService;
            _mapper = mapper;
            _authService = authService; 
            _taskManagementRepo = taskManagementRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasks();
            var taskDTOs = _mapper.Map<IEnumerable<TaskTableDTO>>(tasks);
            foreach (var task in taskDTOs)
            {

                var assignedUser = await _taskManagementRepo.GetAsync(t => t.TaskId == task.TaskId, true);
                if (assignedUser != null)
                {
                    //_logger.LogInformation("Task ID: {TaskId}, Title: {Title}, IsDeleted: {IsDeleted} , userId : {AssignedToUserId}", task.TaskId, task.Title, task.IsDeleted, assignedUser.AssignedToUserId);
                    var user = await _authService.GetUserById(assignedUser.AssignedToUserId!);
                    task.AssignedToUserName = user.UserName!;
                }


            }
            return View(taskDTOs);
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeAnalyticAsync()
        {
            var tasks = await _taskService.GetAllTasks();
            var taskDTOs = _mapper.Map<IEnumerable<TaskTableDTO>>(tasks);
            foreach (var task in taskDTOs)
            {

                var assignedUser = await _taskManagementRepo.GetAsync(t => t.TaskId == task.TaskId, true);
                if (assignedUser != null)
                {
                    //_logger.LogInformation("Task ID: {TaskId}, Title: {Title}, IsDeleted: {IsDeleted} , userId : {AssignedToUserId}", task.TaskId, task.Title, task.IsDeleted, assignedUser.AssignedToUserId);
                    var user = await _authService.GetUserById(assignedUser.AssignedToUserId!);
                    task.AssignedToUserName = user.UserName!;
                }


            }
            return View(taskDTOs);
        }
    }
}
