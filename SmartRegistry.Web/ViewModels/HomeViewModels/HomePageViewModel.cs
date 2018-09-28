using System.Collections.Generic;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.ViewModels.HomeViewModels
{
    public class HomePageViewModel
    {
        public IList<Faculty> Faculties { get; set; }
        public IList<Announcement> Announcements { get; set; }
        public string Description { get; set; }
    }
}
