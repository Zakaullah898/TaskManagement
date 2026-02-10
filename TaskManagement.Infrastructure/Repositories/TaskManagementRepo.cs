using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;



namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskManagementRepo<T> :ITaskManagementRepo<T> where T : class
    {
        private readonly AppDbContext _context;
        private DbSet<T> _dbSet;
        public TaskManagementRepo(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        // Implementing the methods defined in the ITaskManagementRepo<T> interface

        // Create a new entity for the specified type T
        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(DbUpdateException)
            {
                return false;
            }

        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> Filter)
        {
            var entity = await _dbSet.Where(Filter).FirstOrDefaultAsync();
            if(entity == null)
            {
                throw new KeyNotFoundException("This entity is not found");
            }
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return entities;
        }

        public  Task<T> GetAsync(Expression<Func<T, bool>> Filter, bool UseNoTracking = false)
        {
            if(UseNoTracking)
            {
                return  _dbSet.AsNoTracking().Where(Filter).FirstOrDefaultAsync()!;
            }
            else
            {
                return _dbSet.Where(Filter).FirstOrDefaultAsync()!;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }

        }
    }
}
