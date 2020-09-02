using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartRegistry.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ChatController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(HttpContext.User);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                IList<Subject> subjects = _context.Subject
                    .Include(s => s.Lecturer)
                    .Where(s => s.Lecturer.AccountId == userId)
                    .ToList();
            
                return View(subjects);
            }

            //Course course = _context.Course.Include(c => c.)

            return View();
        }

        public IActionResult Reporting(int subjectId)
        {
            return View();
        }

        public IActionResult VideoCalling()
        {
            return View();
        }
    }
}
