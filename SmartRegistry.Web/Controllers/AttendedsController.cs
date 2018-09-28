using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;
using SmartRegistry.Web.ViewModels.DashboardViewModels;

namespace SmartRegistry.Web.Controllers
{
    [Authorize]
    public class AttendedsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendedsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Attendeds
        public async Task<IActionResult> Index(int? subjectId)
        {
            //var applicationDbContext = _context.Attendies.Include(a => a.Schedule).Include(a => a.Student);
            //return View(await applicationDbContext.ToListAsync());

            if (subjectId == null) return View(new DashboardOverviewViewModel());

            //var attendance = from a in _context.Attendies.Include(a => a.Schedule).ThenInclude(a => a.Subject)
            //                 where a.Id == subjectId
            //                 select a;

            var schedules = _context.Schedules.Where(s => s.Id == subjectId);

            var results = schedules.GroupBy(s => s.IsConfirmed).Select(s => new
            {
                label = s.Key,
                value = s.Count()
            });

            //results = from sc in _context.Schedules
            //    group sc by sc.IsConfirmed
            //    into g
            //    select new
            //    {
            //        name = g.Key,
            //        count = g.Count()
            //    };

            var r = from s in _context.Schedules
                group s by s.IsConfirmed
                into g
                select new
                {
                    label = g.Key,
                    value = g.Count(),
                };

            //var results = new
            //{
            //    confirmed = schedules.Count(s => s.IsConfirmed),
            //    unconfirmed = schedules.Count(s => !s.IsConfirmed)
            //};


            return View(new DashboardOverviewViewModel() { AllSchedules = JsonConvert.SerializeObject(results) });
            //return View(new DashboardOverviewViewModel(){ AllSchedules = results.ToString()});
        }

        // GET: Attendeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attended = await _context.Attendies
                .Include(a => a.Schedule)
                .Include(a => a.Student)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (attended == null)
            {
                return NotFound();
            }

            return View(attended);
        }

        // GET: Attendeds/Create
        public IActionResult Create()
        {
            var schedules = _context.Schedules.Include(s => s.Subject).Select(s => new
            {
                Text = $"{s.Subject.Name} - {s.LectureRoom} - {s.CreatedAt.ToShortDateString()}",
                Id = s.Id
            }).AsEnumerable();

            ViewData["ScheduleId"] = new SelectList(/*_context.Schedules*/ schedules , "Id", /*"LectureRoom"*/ "Text");

            var students = _context.Students.Select(s => new
            {
                Text = $"{s.FirstName} - {s.LastName}",
                Id = s.Id
            }).AsEnumerable();

            ViewData["StudentId"] = new SelectList(/*_context.Students*/ students, "Id", /*"FirstName"*/ "Text");
            return View();
        }

        // POST: Attendeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HasAttended,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt,StudentId,ScheduleId")] Attended attended)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);

                var student = _context.Students.FirstOrDefault(s => s.AccountId == userId);

                if (student == null)
                {
                    ModelState.AddModelError("Student", "You not a enrolled student");
                    //return View(ModelState);
                    return View(attended);
                }

                attended.CreatedAt = DateTime.UtcNow;
                attended.LastUpdatedAt = DateTime.UtcNow;
                attended.CreatedBy = userId;
                attended.LastUpdatedBy = userId;
                attended.StudentId = student.Id;
                attended.HasAttended = true;

                _context.Add(attended);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "LectureRoom", attended.ScheduleId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", attended.StudentId);
            return View(attended);
        }

        // GET: Attendeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attended = await _context.Attendies.SingleOrDefaultAsync(m => m.Id == id);
            if (attended == null)
            {
                return NotFound();
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "LectureRoom", attended.ScheduleId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", attended.StudentId);
            return View(attended);
        }

        // POST: Attendeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HasAttended,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt,StudentId,ScheduleId")] Attended attended)
        {
            if (id != attended.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attended);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendedExists(attended.Id))
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
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "LectureRoom", attended.ScheduleId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FirstName", attended.StudentId);
            return View(attended);
        }

        // GET: Attendeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attended = await _context.Attendies
                .Include(a => a.Schedule)
                .Include(a => a.Student)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (attended == null)
            {
                return NotFound();
            }

            return View(attended);
        }

        // POST: Attendeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attended = await _context.Attendies.SingleOrDefaultAsync(m => m.Id == id);
            attended.IsDeleted = true;

            //_context.Attendies.Remove(attended);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendedExists(int id)
        {
            return _context.Attendies.Any(e => e.Id == id);
        }
    }
}
