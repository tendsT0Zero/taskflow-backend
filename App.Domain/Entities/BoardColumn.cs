using App.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class BoardColumn : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; } 

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        // Navigation Properties
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
