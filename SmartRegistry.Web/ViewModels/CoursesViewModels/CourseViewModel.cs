using System.Collections.Generic;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.ViewModels.CoursesViewModels
{
    public class CourseViewModel
    {
        public string Description { get; set; }
        public Course Course { get; set; }
        public IList<Announcement> Announcements { get; set; }
        public IList<Subject> Subjects { get; set; }     
    }
}
