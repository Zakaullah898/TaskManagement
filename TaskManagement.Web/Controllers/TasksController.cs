using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using TaskManagement.Application.CustomException;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Web.Models;

namespace TaskManagement.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly ILogger<TasksController> _logger;
        IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private readonly ITaskManagementRepo<UserRole> _userRolesRepo;
        private readonly ITaskManagementRepo<AssignUserRole> _assignUserRoleRepo;
        private readonly ITaskManagementRepo<TaskAssignments> _taskManagementRepo;

        public TasksController(
            ILogger<TasksController> logger,
            ITaskService taskService,
            IMapper mapper,
            IAuthService authService,
            ITaskManagementRepo<UserRole> userRolesRepo,
            ITaskManagementRepo<AssignUserRole> assignUserRoleRepo,
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
        public IActionResult Index()
        {
            return View();
        }
        // endpoint for creating tasks
        [Authorize(Roles = "Manager")]
        public IActionResult CreateTask()
        {
            return View();
        }
        // endpoint for creating tasks
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskTableDTO model)
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                _logger.LogInformation("User ID: {UserId}", userId);
                model.CreatedByUserId = userId;


                if (ModelState.IsValid)
                {
                    model.UpdatedAt = DateTime.Now;
                    model.Status = "New";
                    model.IsDeleted = false;
                    model.CreatedAt = DateTime.Now;
                    var Task = _mapper.Map<TaskTable>(model);
                    // Logic to create a new task
                    bool isTaskCreated = await _taskService.CreateTask(Task);
                    if (isTaskCreated)
                    {
                        TempData["Create"] = "Task Created successfully.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "An error occurred while creating a task.";
                        return View(model);
                    }

                }
                else
                {
                    return View(model);
                }
            }
            catch (ConflictException c)
            {
                _logger.LogError("Error : ", c);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a task.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }
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

        // Go to assign task page
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userDto = new List<UserDTO>();
                var users = _authService.GetAllUsers().Result;
                var roles = _userRolesRepo.GetAllAsync().Result;
                foreach (var user in users)
                {
                    var role = await _userRolesRepo.GetAsync(r => r.RoleName == "Employee");
                    if (role != null)
                    {
                        var assignedRoles = await _assignUserRoleRepo.GetAsync(ar => ar.UserId == user.Id);
                        _logger.LogInformation("Assign role {assignedRoles}", assignedRoles.UserId);
                        if (user.Id == assignedRoles.UserId && assignedRoles.RoleId == role.RoleId)
                        {
                            _logger.LogInformation("User {user} has role {role}", user.UserName, role.RoleName);
                            userDto.Add(_mapper.Map<UserDTO>(user));
                        }
                    }
                }
                return PartialView("_AssignTaskModalPartial", userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the assign task page.");
                TempData["ErrorMessage"] = "An error occurred while loading the assign task page.";
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }
        }
        // Assigning task endpoint
        [HttpPost]
        public async Task<IActionResult> AssignTask(int taskId, string AssigneeId)
        {
            try
            {

                var currentUserId = User.FindFirst("UserId")?.Value;
                // Logic to assign task to users can be added here
                await _taskService.AssignTaskToUser(taskId, AssigneeId, currentUserId!);
                TempData["SuccessMessage"] = "Task assigned successfully.";
                TempData["AlertType"] = "success";
                return RedirectToAction("TaskDetails", new { id = taskId });
            }
            catch (ConflictException ce)
            {
                _logger.LogError(ce, "Conflict error while assigning the task.");
                TempData["ErrorMessage"] = ce.Message;
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning the task.");
                TempData["ErrorMessage"] = "An error occurred while assigning the task.";
                TempData["AlertType"] = "error";
                return RedirectToAction("TaskDetails", new { id = taskId });
            }
        }

        // Getting all assigned tasks
        [HttpGet]
        public async Task<IActionResult> GetAllAssignedTask()
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
                    _logger.LogInformation("Assigned User: {UserName}", user.UserName);
                }


            }
            return PartialView("_AssignedTaskPartial", taskDTOs);
        }

        // updating Tasks view 
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult UpdateTask(int id)
        {
            try
            {
                var task = _taskService.GetTaskById(id).Result;
                var taskDTO = _mapper.Map<TaskTableDTO>(task);
                return View(taskDTO);
            }
            catch (KeyNotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                TempData["AlertType"] = "error";
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the task for update.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }


        // Updating Tasks endpoint
        [Authorize(Roles = "Manager")]
        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskTableDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var taskId = RouteData.Values["id"];
                    if (taskId != null)
                    {
                        model.TaskId = Convert.ToInt32(taskId);
                        var taskModel = _mapper.Map<TaskTable>(model);
                        // Logic to update the task
                        bool isTaskUpdated = await _taskService.UpdateTask(taskModel);
                        if (isTaskUpdated)
                        {
                            TempData["Update"] = "Task updated successfully.";
                            return RedirectToAction("Index", "Home");
                        }
                        return View(model);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Task ID is missing.";
                        TempData["AlertType"] = "error";
                        return View(model);
                    }

                }
                else
                {
                    return View(model);
                }
            }
            catch (KeyNotFoundException k)
            {
                _logger.LogError("Error : ", k);
                TempData["ErrorMessage"] = k.Message;
                TempData["AlertType"] = "error";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a task.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
        // partial updating tasks status
        [HttpPatch]
        public async Task<IActionResult> PartialUpdateTaskStatus(int taskId, [FromBody] JsonPatchDocument<TaskTableDTO> patchDocumnent)
        {
            try
            {
                if (patchDocumnent == null)
                {
                    return BadRequest("Invalid patch document.");
                }
                // Logic to partially update task status can be added here
                var ExistingTask = await _taskService.GetTaskById(taskId);
                if (ExistingTask == null)
                {
                    return NotFound("Task not found.");
                }
                var taskDTOs = _mapper.Map<TaskTableDTO>(ExistingTask);
                patchDocumnent.ApplyTo(taskDTOs, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedTask = _mapper.Map<TaskTable>(taskDTOs);
                var isUpdated = await _taskService.PartialUpdatingTaskStatus(updatedTask);

                if (isUpdated)
                {
                    return Ok(new { message = "Task status updated successfully." });
                }
                else
                {
                    return BadRequest("Failed to update task status.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the task status.");
                return StatusCode(500, "An error occurred while updating the task status.");
            }

        }

        // Deleting task endpoint
        [Authorize(Roles = "Manager,Admin")]
        [HttpDelete("Dashboard/DeleteTask")]
        //[Route("Dashboard/DeleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                // Logic to delete a task can be added here
                var isDeleted = await _taskService.DeleteTask(id);
                if (isDeleted)
                {
                    TempData["Delete"] = "Task deleted successfully.";
                    return Ok("Task deleted successfully.");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the task.";
                    TempData["AlertType"] = "error";
                    return BadRequest("Failed to delete the task.");
                }
            }
            catch (KeyNotFoundException k)
            {
                _logger.LogError("Error : ", k);
                TempData["ErrorMessage"] = k.Message;
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the task.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
