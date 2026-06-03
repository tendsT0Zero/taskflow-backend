using App.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class ActivityLog : BaseEntity
    {
        public string ActionText { get; set; } = string.Empty;
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}
