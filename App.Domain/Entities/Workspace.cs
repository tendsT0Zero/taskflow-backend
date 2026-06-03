using App.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class Workspace : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int OwnerId { get; set; }
        public AppUser Owner { get; set; } = null!;

        // Navigation Properties
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
