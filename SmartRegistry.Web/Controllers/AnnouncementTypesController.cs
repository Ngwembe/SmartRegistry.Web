using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.Controllers
{
    [Authorize]
    public class AnnouncementTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AnnouncementTypesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AnnouncementType
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnnouncementType.ToListAsync());
        }

        // GET: AnnouncementType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcementType = await _context.AnnouncementType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (announcementType == null)
            {
                return NotFound();
            }

            return View(announcementType);
        }

        // GET: AnnouncementType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnnouncementType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt")] AnnouncementType announcementType)
        {
            if (ModelState.IsValid)
            {
                announcementType.CreatedAt = DateTime.UtcNow;
                announcementType.LastUpdatedAt = DateTime.UtcNow;
                announcementType.CreatedBy = _userManager.GetUserId(HttpContext.User);
                announcementType.LastUpdatedBy = _userManager.GetUserId(HttpContext.User);

                _context.Add(announcementType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(announcementType);
        }

        // GET: AnnouncementType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcementType = await _context.AnnouncementType.SingleOrDefaultAsync(m => m.Id == id);
            if (announcementType == null)
            {
                return NotFound();
            }
            return View(announcementType);
        }

        // POST: AnnouncementType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt")] AnnouncementType announcementType)
        {
            if (id != announcementType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcementType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementTypeExists(announcementType.Id))
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
            return View(announcementType);
        }

        // GET: AnnouncementType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcementType = await _context.AnnouncementType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (announcementType == null)
            {
                return NotFound();
            }

            return View(announcementType);
        }

        // POST: AnnouncementType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcementType = await _context.AnnouncementType.SingleOrDefaultAsync(m => m.Id == id);
            //_context.AnnouncementType.Remove(announcementType);
            announcementType.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementTypeExists(int id)
        {
            return _context.AnnouncementType.Any(e => e.Id == id);
        }
    }
}
