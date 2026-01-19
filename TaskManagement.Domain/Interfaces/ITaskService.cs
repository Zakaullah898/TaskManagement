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
        Task<IEnumerable<TaskTable>> GetAllTasks();
        Task<bool> CreateTask(TaskTable model);

    }
}
