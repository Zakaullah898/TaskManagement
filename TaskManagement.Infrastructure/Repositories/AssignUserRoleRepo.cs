using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    public class AssignUserRoleRepo : TaskManagementRepo<AssignUserRole>, IAssignUserRoleRepo
    {
        private readonly AppDbContext _context;
        public AssignUserRoleRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<int>> GettingRoleIds(Expression<Func<AssignUserRole, bool>> Filter, bool useNoTracking = false)
        {
            if(useNoTracking)
            {
                var roleIdsNoTracking = await _context.Set<AssignUserRole>().AsNoTracking().Where(Filter).Select(ar => ar.RoleId).ToListAsync();
                return roleIdsNoTracking;
            }
            else
            {
                return await _context.Set<AssignUserRole>().Where(Filter).Select(ar => ar.RoleId).ToListAsync();
            }
        }
    }
}
