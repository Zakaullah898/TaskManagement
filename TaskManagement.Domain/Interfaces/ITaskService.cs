
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskService
    {

        Task<TaskTable> GetTaskById(int id);
        // assign task to user
        Task<bool> AssignTaskToUser(int taskId, string userId, string currentLoginUser);
        Task<IEnumerable<TaskTable>> GetAllTasks();
        Task<bool> CreateTask(TaskTable model);
        Task<bool> UpdateTask(TaskTable model);
        // partial updating task status
        Task<bool> PartialUpdatingTaskStatus(TaskTable task);
        Task<bool> Delete(int id);



    }
}
