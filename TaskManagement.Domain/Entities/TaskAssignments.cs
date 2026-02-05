using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class TaskAssignments
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string? AssignedToUserId { get; set; }
        public string? AssignedByUserId { get; set; }
        public DateTime AssignedAt { get; set; }
        public bool isActive { get; set; }

        // Navigation properties
        public virtual AppUser? AssignedToUser { get; set; }
        public virtual TaskTable? Task { get; set; }

    }
}
