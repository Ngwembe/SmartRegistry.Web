using System;
using System.Collections.Generic;
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

namespace SmartRegistry.Web.Controllers
{
    [Authorize]
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnnouncementsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Announcement
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Announcement.Include(a => a.Lecturer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Announcement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement
                .Include(a => a.Lecturer)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: Announcement/Create
        public IActionResult Create()
        {
            //this week's lecture, the 31st of May 2018 is cancelled due to a meeting that will be held at the department. I'll do a virtual tutorial and upload it on MyTutor for you to download. It will cover Chapter 5 & 6. Enjoy your weekend good people.
            ViewData["LecturerId"] = new SelectList(_context.Lecturer.Select(s => new
            {
                Id = s.Id,
                FullName = $"{s.FirstName} {s.LastName}"
            })
                , "Id", "FullName");
            //, "Id", "FirstName");

            //var types = GetEnumSelectList<AnnouncementType>();
            //ViewData["AnnouncementId"] = types; //new SelectList(_context.Lecturer, "Id", "FirstName");
            return View(new Announcement());
        }

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            //return (Enum.GetValues(typeof(T)).Cast<T>().Select(e => new SelectListItem()
            //{
            //    Text = e.ToString(),
            //    Value = e.ToString()
            //}).ToList());

            return (Enum.GetValues(typeof(T)).Cast<int>().Select(e => new SelectListItem()
            {
                Text = Enum.GetName(typeof(T), e),
                Value = e.ToString()
            })).ToList();
        }

        // POST: Announcement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,LecturerId,Message,AnnouncementTypeId,AnnouncementTypeOwnerId,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserId(HttpContext.User);

                announcement.CreatedBy = user;
                announcement.CreatedAt = DateTime.UtcNow;
                announcement.LastUpdatedBy = user;
                announcement.LastUpdatedAt=  DateTime.UtcNow;

                _context.Add(announcement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", announcement.LecturerId);
            return View(announcement);
        }

        public string GetCategories(string searchKey)
        {
            var categories = new List<AnnouncementType>();

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                categories = _context.AnnouncementType.Where(a => a.Text.ToLowerInvariant().Contains(searchKey.ToLowerInvariant())).ToList();
                return JsonConvert.SerializeObject(categories.Select(c => new { id = c.Id, text = c.Text }).ToList());
            }

            categories = _context.AnnouncementType.ToList();
            return JsonConvert.SerializeObject(categories.Select(c => new { id = c.Id, text = c.Text }).ToList());            
        }

        public string GetPossibleAnnouncementOwners(string category, string searchKey)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                dynamic announcements = null;
                switch (category)
                {
                    case "1":
                        announcements = _context.Announcement.ToList();
                        break;
                    case "2":
                        announcements = _context.Faculty.Where(f => f.Name.ToLowerInvariant().Contains(searchKey.ToLowerInvariant())).Select(f => new { id = f.Id, text = $"{f.Code} - {f.Name}" }).ToList();
                        break;
                    case "4":
                        announcements = _context.Course.Where(c => c.Name.ToLowerInvariant().Contains(searchKey.ToLowerInvariant())).Select(f => new { id = f.Id, text = $"{f.Code} - {f.Name}" }).ToList();
                        break;
                    case "3":
                        announcements = _context.Department.Where(d => d.Name.ToLowerInvariant().Contains(searchKey.ToLowerInvariant())).Select(f => new { id = f.Id, text = $"{f.Code} - {f.Name}" }).ToList();
                        break;
                    case "5":
                        announcements = _context.Subject.Where(s => s.Name.ToLowerInvariant().Contains(searchKey.ToLowerInvariant())).Select(f => new { id = f.Id, text = $"{f.Code} - {f.Name}" }).ToList();
                        break;
                }

                return JsonConvert.SerializeObject(announcements);
            }

            return string.Empty;
        }

        // GET: Announcement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement.SingleOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }
            ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", announcement.LecturerId);
            return View(announcement);
        }

        // POST: Announcement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,LecturerId,Message,AnnouncementType,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt")] Announcement announcement)
        {
            if (id != announcement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcement.Id))
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
            ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", announcement.LecturerId);
            return View(announcement);
        }

        // GET: Announcement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement
                .Include(a => a.Lecturer)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await _context.Announcement.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Announcement.Remove(announcement);
            announcement.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcement.Any(e => e.Id == id);
        }
    }
}
