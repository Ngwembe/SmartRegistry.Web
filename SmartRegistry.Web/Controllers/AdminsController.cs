using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.Controllers
{
    //[Authorize]
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminsController(ApplicationDbContext context,RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        private async Task SeedRoles()
        {
            if ((await _roleManager.FindByNameAsync("System Admin")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "System Admin" });

                var role = new IdentityRole();
                var userId = _userManager.GetUserId(HttpContext.User);
                var user = await _userManager.FindByIdAsync(userId);
                await _userManager.AddToRoleAsync(user, "System Admin");
            }

            if ((await _roleManager.FindByNameAsync("Admin")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });

                var role = new IdentityRole();
                var userId = _userManager.GetUserId(HttpContext.User);
                var user = await _userManager.FindByIdAsync(userId);
                await _userManager.AddToRoleAsync(user, "System Admin");
            }

            if ((await _roleManager.FindByNameAsync("Lecturer")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Lecturer" });

                var role = new IdentityRole();
                var userId = _userManager.GetUserId(HttpContext.User);
                var user = await _userManager.FindByIdAsync(userId);
                await _userManager.AddToRoleAsync(user, "System Admin");
            }

        }


        // GET: Admins
        //#TODO: Update the student's model property [IsComplete] upon assigning the sensor ID
        public async Task<ActionResult> Index()
        {
            await SeedRoles();

           ViewData["Admins"] = await _userManager.GetUsersInRoleAsync("Admin");
            //var systemAdmins = await _userManager.GetUsersInRoleAsync("System Admin");

            ViewData["StudentIds"] = new SelectList(_context.Student.Select(u => new
            {
                Id = u.StudentId,
                Name = $"{u.FirstName} {u.LastName} ({u.StudentNumber})"
            }), "Id", "Name");

            ViewData["SensorIds"] = new SelectList(_context.Sensor.Where(s => !s.IsAssigned).Select(u => new
            {
                Id = u.SensorId,
                Name = $"Unassigned Sensor # {u.SensorId}"
            }), "Id", "Name");

            return View();
        }



        // GET: Admins/Details/5
        public async Task<ActionResult> Details(int id)
        {
            //var role = new IdentityRole();
            //var userId = _userManager.GetUserId(HttpContext.User);
            //var user = await _userManager.FindByIdAsync(userId);
            //await _userManager.AddToRoleAsync(user, "System Admin");

            return View(nameof(Index));
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            var lecturers = new SelectList(_context.Lecturer.Select(u => new
            {
                Id = u.LecturerId,
                Name = $"{u.FirstName} {u.LastName}"
            }), "Id", "Name");

            ViewData["Lecturer"] = lecturers;


            ViewData["Roles"] =  new SelectList(_roleManager.Roles, "Id", "NormalizedName");

            return View(new Lecturer());
        }

        // POST: Admins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,AccountId")] Lecturer lecturer)
        {
            try
            {
                var user = _context.Lecturer.FirstOrDefault(u => u.LecturerId == lecturer.LecturerId);

                if (user == null)
                    return View(lecturer);

                //var r = await _userManager.FindByIdAsync(user.AccountId);
                //var adminId = _userManager.GetUserId(HttpContext.User);
                var assignee = await _userManager.FindByIdAsync(user.AccountId);
                //var adminId = _userManager.GetUserId(HttpContext.User);

                var selectedRole = await _roleManager.FindByIdAsync(lecturer.AccountId);  //AccountId used to store the RoleId from the dropdown menu item selected

                //if(selectedRole != null && !string.IsNullOrWhiteSpace(adminId))
                if (selectedRole != null && !string.IsNullOrWhiteSpace(user.AccountId))
                    await _userManager.AddToRoleAsync(assignee/*await _userManager.FindByIdAsync(user.AccountId)*/, selectedRole.Name);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admins/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> RemoveFromRole(string userId, string roleName)
        {
            try
            {

                //var user = await _userManager.GetUserAsync(HttpContext.User);

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return RedirectToAction(nameof(Index));

                await _userManager.RemoveFromRoleAsync(user/*await _userManager.GetUserIdAsync(HttpContext.User)*/, roleName);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}