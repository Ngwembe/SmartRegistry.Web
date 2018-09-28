using System.Collections.Generic;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.ViewModels.DepartmentViewModels
{
    public class DepartmentViewModel
    {
        public Department Department { get; set; }
        public string Description { get; set; }

        public IList<Announcement> Announcements { get; set; }
        public IList<Course> Courses { get; set; }
    }
}
