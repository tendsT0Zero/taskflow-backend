using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int Order { get; set; }
        public int ColumnId { get; set; }
        public int? AssigneeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
