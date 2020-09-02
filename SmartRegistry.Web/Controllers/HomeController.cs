using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;
using SmartRegistry.Web.ViewModels.HomeViewModels;

namespace SmartRegistry.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var faculties = _context.Faculty.ToList();
            var announcements = _context.Announcement.Where(a => a.AnnouncementTypeId == 1).ToList();

            var homepageVM = new HomePageViewModel()
            {
                Announcements = announcements,
                Faculties = faculties,
                Description = "This is institution's brief description... () To be pulled from the DB"
            };

            return View(homepageVM);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
