using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskManagement.Application.CustomException;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Web.Models;

namespace TaskManagement.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IMapper _mapper;
        private readonly ITaskService _taskService;

        public HomeController(ILogger<HomeController> logger, ITaskService taskService, IMapper mapper)
        {
            _logger = logger;
            _taskService = taskService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CreateTask()
        {
            return View();
        }
        // Creating Tasks endpoint
        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskTableDTO model)
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                _logger.LogInformation("User ID: {UserId}", userId);
                model.CreatedByUserId = userId;
                model.UpdatedAt = DateTime.Now;
                model.Status = "New";
                model.IsDeleted = false;
                model.CreatedAt = DateTime.Now;

                if (ModelState.IsValid)
                {
                    var TaskModel = _mapper.Map<TaskTable>(model);
                    // Logic to create a new task
                    bool isTaskCreated = await _taskService.CreateTask(TaskModel);
                    if (isTaskCreated)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return View(model);

                }
            }
            catch (ConflictException c)
            {
                _logger.LogError("Error : ",c);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a task.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return View(model);
        }

        //// Getting all tasks endpoint
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasks();
            var taskDTOs = _mapper.Map<IEnumerable<TaskTableDTO>>(tasks);
            return PartialView("_ItemsPartial", taskDTOs);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
