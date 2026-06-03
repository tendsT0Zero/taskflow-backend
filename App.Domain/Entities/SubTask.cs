using App.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class SubTask : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;
    }
}
