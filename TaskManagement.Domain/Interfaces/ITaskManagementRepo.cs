using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskManagementRepo<T> where T: class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> Filter, bool UseNoTracking = false);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<T> DeleteAsync(Expression<Func<T, bool>> Filter);
    }
}
