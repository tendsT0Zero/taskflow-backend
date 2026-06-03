using App.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; } = null!;

        // Navigation Properties
        public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();
    }
}
