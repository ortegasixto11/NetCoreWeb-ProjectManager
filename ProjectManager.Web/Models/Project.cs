using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<ProjectWorkflow> ProjectWorkflows { get; set; }
    }
}
