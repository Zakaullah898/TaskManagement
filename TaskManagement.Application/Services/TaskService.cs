using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.CustomException;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskManagementRepo<TaskTable> _taskRepository;
        private readonly ITaskManagementRepo<TaskAssignments> _taskAssignmentsRepository;
        IMapper _mapper;
        public TaskService(ITaskManagementRepo<TaskTable> taskRepository, ITaskManagementRepo<TaskAssignments> taskAssignmentsRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _taskAssignmentsRepository = taskAssignmentsRepository;
            _mapper = mapper;
        }

        // Get all tasks
        public async Task<IEnumerable<TaskTable>> GetAllTasks()
        {
            return await _taskRepository.GetAllAsync();
        }

        // Create a new task
        public async Task<bool> CreateTask(TaskTable model)
        {
            var existingTask = await _taskRepository.GetAsync(t => t.TaskId == model.TaskId, true);
            if (existingTask != null)
            {
                throw new ConflictException("Task Already is created");
            }
            else
            {
                // Logic to create task

                var isCreated = await _taskRepository.CreateAsync(model);
                if (isCreated)
                {
                    return true;
                }
                return false;
            }
        }

        // Update an existing task
        public async Task<bool> UpdateTask(TaskTable model)
        {
            var existingTask = await _taskRepository.GetAsync(t => t.TaskId == model.TaskId, true);
            if (existingTask == null)
            {
                throw new KeyNotFoundException("Task not found");
            }
            else
            {
                // Logic to update task
                existingTask.Title = model.Title;
                existingTask.Description = model.Description;
                existingTask.priority = model.priority;
                existingTask.DueDate = model.DueDate;
                existingTask.UpdatedAt = DateTime.UtcNow;
                var isUpdated = await _taskRepository.UpdateAsync(existingTask);
                if (isUpdated)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<TaskTable> GetTaskById(int id)
        {
            var existingTask = await _taskRepository.GetAsync(t => t.TaskId == id, true);
            if (existingTask == null)
            {
                throw new KeyNotFoundException("Task not found");
            }
            return existingTask;
        }

        public async Task<bool> Delete(int id)
        {
            var existingTask =  _taskRepository.GetAsync(t => t.TaskId == id, true).Result;
            if (existingTask == null)
            {
                throw new KeyNotFoundException("Task not found");
            }
            existingTask.IsDeleted = true;
            bool isDeleted = await _taskRepository.UpdateAsync(existingTask);
            if (isDeleted)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AssignTaskToUser(int taskId, string userId, string currentLoginUser)
        {
            
            var existingAssignment = await _taskAssignmentsRepository.GetAsync(ta => ta.TaskId == taskId);
            if (existingAssignment != null)
            {
                throw new ConflictException("Task is already assigned to a user.");
            }

                var assignment = new TaskAssignments
            {
                TaskId = taskId,
                AssignedToUserId = userId,
                AssignedByUserId = currentLoginUser,
                AssignedAt = DateTime.UtcNow,
                isActive = true
            };
                var isCreated = await _taskAssignmentsRepository.CreateAsync(assignment);
                if (isCreated)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        // Partial updating task status
        public async Task<bool> PartialUpdatingTaskStatus(TaskTable task)
        {
            
            if(task == null )
            {
                throw new ArgumentNullException(nameof(task));
            }
            
            var isUpdated = await  _taskRepository.UpdateAsync(task);
            if (isUpdated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteTask(int id)
        {
            var existingTask = await _taskRepository.GetAsync(t => t.TaskId == id, true);
            if (existingTask == null)
            {
                throw new KeyNotFoundException("Task not found");
            }
                var isDeleted = await _taskRepository.DeleteAsync(t => t.TaskId == existingTask.TaskId);
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
}
