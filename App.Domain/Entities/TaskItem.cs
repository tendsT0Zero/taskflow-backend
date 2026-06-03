using App.Domain.Common;
using App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; } 


        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public int ColumnId { get; set; }
        public BoardColumn Column { get; set; } = null!;

        public int? AssigneeId { get; set; }
        public AppUser? Assignee { get; set; }

        // Navigation Properties
        public ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }
}
