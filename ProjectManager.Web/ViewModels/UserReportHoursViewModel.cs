using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.ViewModels
{
    public class UserReportHoursViewModel
    {
        public string ProjectId { get; set; }
        public string UserId { get; set; }
        [DisplayName("Hours")]
        public double ReportedHours { get; set; }
    }
}
