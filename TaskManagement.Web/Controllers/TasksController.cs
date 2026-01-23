using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly ILogger<TasksController> _logger;
        IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;

        public TasksController(ILogger<TasksController> logger, ITaskService taskService, IMapper mapper, IAuthService authService)
        {
            _logger = logger;
            _taskService = taskService;
            _mapper = mapper;
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TaskDetails(int id)
        {
            try
            {
                // Logic to get task details by id can be added here
                var task = _taskService.GetTaskById(id).Result;
                var user = _authService.GetUserById(task.CreatedByUserId!).Result;
                var taskDTOs = _mapper.Map<TaskTableDTO>(task);
                taskDTOs.CreatedByUserName = user.UserName;
                return View(taskDTOs);

            }
            catch (KeyNotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                TempData["AlertType"] = "error";
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the task details.");
                return View("Error");
            }
        }

    }
}
