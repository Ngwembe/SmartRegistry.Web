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
using SmartRegistry.Web.Interfaces;
using SmartRegistry.Web.Models;
using SmartRegistry.Web.ViewModels.SubjectViewModels;

namespace SmartRegistry.Web.Controllers
{
    //[Authorize]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReportingHandler _reportingHandler;
        private readonly IEmailSender _emailSender;

        public SubjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IReportingHandler reportingHandler, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _reportingHandler = reportingHandler;
            _emailSender = emailSender;
        }

        // GET: Subject
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(this.User);

            if (user == null) return Create();

            var userId = await _userManager.GetUserIdAsync(user);

            if (userId == null) return Create();

            var lecturer = await _context.Lecturer.FirstOrDefaultAsync(l => l.AccountId == userId);

            var applicationDbContext = _context.Subject
                                               .Include(s => s.Course)
                                               .Include(s => s.Lecturer)
                                               .Where(s => s.LecturerId == lecturer.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> GetAllEnrolled(int id) //subjectId
        {
            var students = await _context.EnrolledSubject
                .Where(s => s.SubjectId == id)
                .Include(s => s.Student)
                .Include(s => s.Subject)
                //.ThenInclude(s => s.EnrolledSubject)
                //.Where(s=> s.SubjectId == id)
                .Select(en => new EnrolledStudentViewModel
                 {
                    SubjectId = id,
                    StudentId = en.StudentId,
                    FirstName = en.Student.FirstName,
                    LastName = en.Student.LastName,
                    StudentNumber = en.Student.StudentNumber,
                    SubjectCode = en.Subject.Code,
                    SubjectName = en.Subject.Name
                }).ToListAsync();

            //var students = from st in _context.Student
            //               join es in _context.EnrolledSubject on st.Id equals es.StudentId
            //               join sub in _context.Subject on sub.Id equals es.SubjectId
            //               select st;

            return View(students);
        }

        // GET: Subject/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .Include(s => s.Course)
                .Include(s => s.Lecturer)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }
            
            var user = await _userManager.GetUserAsync(this.User);

            if (user != null)
            {
                var userId = await _userManager.GetUserIdAsync(user);

                var student = await _context.Student.FirstOrDefaultAsync(s => s.AccountId == userId);

                if (student == null)
                    return View(subject);

                var exists = await _context.EnrolledSubject
                    .FirstOrDefaultAsync(s => s.SubjectId == id && s.StudentId == student.Id);

                var isEnrolled = exists != null;

                ViewData["AlreadyEnrolled"] = isEnrolled;
            }


            //if (user == null)
            //    return RedirectToAction("Login", "Account", new { returnUrl = $"/Subject/Details/{id}" });

            //var userId = await _userManager.GetUserIdAsync(user);

            //var student = await _context.Student.FirstOrDefaultAsync(s => s.AccountId == userId);

            //if(student == null)
            //    return View(subject);

            //var exists = await _context.EnrolledSubject
            //    .FirstOrDefaultAsync(s => s.SubjectId == id && s.StudentId == student.Id);

            //var isEnrolled = exists != null;

            //ViewData["AlreadyEnrolled"] = isEnrolled;

            return View(subject);
        }

        // GET: Subject/Create
        //[Authorize]
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code");
            ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName");
            return View();
        }

        // POST: Subject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,CourseId,LecturerId")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserId(HttpContext.User);
                subject.CreatedAt = DateTime.UtcNow;
                subject.CreatedBy = user;
                subject.LastUpdatedBy = user;

                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code", subject.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", subject.LecturerId);
            return View(subject);
        }

        // GET: Subject/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject.SingleOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code", subject.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", subject.LecturerId);
            return View(subject);
        }

        // POST: Subject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt,CourseId,LecturerId")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code", subject.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", subject.LecturerId);
            return View(subject);
        }

        // GET: Subject/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .Include(s => s.Course)
                .Include(s => s.Lecturer)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subject.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Subject.Remove(subject);            
            subject.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subject.Any(e => e.Id == id);
        }

        public string GetMatchingSubjects(string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                var subjects = _context.Subject.Take(10).Select(s => new
                {
                    id = s.Id,
                    text = $"{s.Name} ({s.Code})"
                }).ToList();
                return JsonConvert.SerializeObject(subjects);
            }

            var filtered = _context.Subject.Take(5).Where(s => s.Name.ToLowerInvariant().Contains(searchKey.ToLowerInvariant())).Select(s => new
            {
                id = s.Id,
                text = $"{s.Name} ({s.Code})"
            }).ToList();
            return JsonConvert.SerializeObject(filtered);
        }       
    }    
}
