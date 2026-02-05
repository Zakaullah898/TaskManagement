using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IAssignUserRoleRepo : ITaskManagementRepo<AssignUserRole>
    {
        Task<List<int>> GettingRoleIds(Expression<Func<AssignUserRole, bool>> Filter, bool useNoTracking = false);
    }
}
