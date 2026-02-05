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

        
        // go to delete page

        [HttpGet]
        public async Task<IActionResult> DeletedTasks() {
            var tasks = await _taskService.GetAllTasks();
            var taskDTOs = _mapper.Map<IEnumerable<TaskTableDTO>>(tasks);
            return PartialView("_DeletedTasksPartial", taskDTOs);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                bool isDeleted = await _taskService.Delete(id);
                if (isDeleted)
                {
                    return View("Index");
                }
                else
                {
                    return View("Index");
                }
            }
            catch (KeyNotFoundException k)
            {
                _logger.LogError("Error : ", k);
                TempData["ErrorMessage"] = k.Message;
                TempData["AlertType"] = "error";
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error : ", ex);
                TempData["ErrorMessage"] = ex.Message;
                TempData["AlertType"] = "error";
                return View("Index");
            }
        }
        public IActionResult Help()
        {
            ViewBag.GuiddeUrl = "https://your.guidde.link/embed/video-id";
            return View();
        }


    }
}
