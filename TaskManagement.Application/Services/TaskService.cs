using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Application.CustomException;
using TaskManagement.Application.DTOs;
namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskManagementRepo<TaskTable> _taskRepository;
        public TaskService(ITaskManagementRepo<TaskTable> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // Get all tasks
        public async Task<IEnumerable<TaskTable>> GetAllTasks()
        { 
            return await _taskRepository.GetAllAsync();
        }

        // Create a new task
        public async Task<bool> CreateTask(TaskTable model)
        {
            var existingTask = await _taskRepository.GetAsync(t => t.TaskId == model.TaskId,true);
            if(existingTask != null )
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

        
    }
}
