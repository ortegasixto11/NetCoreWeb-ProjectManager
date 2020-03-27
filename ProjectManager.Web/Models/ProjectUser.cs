using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.Models
{
    public class ProjectUser
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public Project Project { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
