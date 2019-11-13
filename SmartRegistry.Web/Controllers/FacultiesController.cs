using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;
using SmartRegistry.Web.ViewModels.FacultiesViewModels;

namespace SmartRegistry.Web.Controllers
{
    //[Authorize]
    public class FacultiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FacultiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Faculty
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faculty.ToListAsync());
        }

        // GET: Faculty/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculty
                .SingleOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                return NotFound();
            }

            var departments = _context.Department.Where(d => d.FacultyId == id).ToList();
            //var announcements = _context.Announcement.Where(a => a.AnnouncementType == AnnouncementType.Faculty).ToList();

            var facultyVM = new FacultyViewModel()
            {
                Faculty = faculty,
                //Announcement = announcements,
                Departments = departments,
                Description = "this Faculty brings equip students with necesssary skills to ..."
            };

            return View(facultyVM);
        }

        // GET: Faculty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,Description,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                faculty.CreatedAt =DateTime.UtcNow;
                faculty.LastUpdatedAt = DateTime.Now;
                faculty.CreatedBy = _userManager.GetUserId(HttpContext.User);

                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculty/Edit/5
        //[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculty.SingleOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // POST: Faculty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt")] Faculty faculty)
        {
            if (id != faculty.FacultyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    faculty.LastUpdatedAt = DateTime.Now;
                    faculty.LastUpdatedBy = _userManager.GetUserId(HttpContext.User);

                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.FacultyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculty/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculty
                .SingleOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: Faculty/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _context.Faculty.SingleOrDefaultAsync(m => m.FacultyId == id);
            //_context.Faculty.Remove(faculty);
            faculty.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyExists(int id)
        {
            return _context.Faculty.Any(e => e.FacultyId == id);
        }
    }
}
