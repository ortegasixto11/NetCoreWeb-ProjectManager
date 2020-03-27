using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }
        public int DNI { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }
}
