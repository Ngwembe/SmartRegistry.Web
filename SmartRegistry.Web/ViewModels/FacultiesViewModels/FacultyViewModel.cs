using System.Collections.Generic;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.ViewModels.FacultiesViewModels
{
    public class FacultyViewModel
    {
        public string Description { get; set; }
        public Faculty Faculty { get; set; }
        public IList<Announcement> Announcements { get; set; }
        public IList<Department> Departments { get; set; }        
    }
}
