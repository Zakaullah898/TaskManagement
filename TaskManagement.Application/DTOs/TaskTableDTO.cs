using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs
{
    public class TaskTableDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? priority { get; set; }
        public string? Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
