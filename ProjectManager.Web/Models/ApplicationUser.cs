using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int DNI { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }
        [NotMapped]
        [DisplayName("Password")]
        public string PasswordNotMapped { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<ProjectWorkflow> ProjectWorkflows { get; set; }

    }
}
