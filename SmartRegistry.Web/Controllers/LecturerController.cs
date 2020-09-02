using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.ViewModels.DashboardViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartRegistry.Web.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            return View(new AdminStatsDashboardViewModel() { SiteVisits = 50, TotalStudents = await _context.Student.CountAsync() });
        }
    }
}
