using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IReportingHandler _reportingHandler;
        private readonly IEmailSender _emailSender;

        public SubjectsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IReportingHandler reportingHandler, IEmailSender emailSender)
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
            ViewData["IsStudent"] = false;

            var user = await _userManager.GetUserAsync(this.User);

            if (user == null) return Create();

            var userId = await _userManager.GetUserIdAsync(user);

            if (userId == null) return Create();

            var userIdentity = await _userManager.FindByIdAsync(userId);

            if (await _userManager.IsInRoleAsync(userIdentity, "System Admin") || await _userManager.IsInRoleAsync(userIdentity, "Admin"))
            {
                var subjects = _context.Subject
                    .Include(s => s.Course)
                    .Include(s => s.Lecturer);

                return View(await subjects.ToListAsync());
            }
            else if (await _userManager.IsInRoleAsync(userIdentity, "Lecturer"))
            {
                //var lecturer = await _context.Lecturer.FirstOrDefaultAsync(l => l.AccountId == userId);
                var subjects = _context.Subject
                                                   .Include(s => s.Course)
                                                   .Include(s => s.Lecturer)
                                                   .Where(s => s.Lecturer.AccountId == userId);
                return View(await subjects.ToListAsync());
            }
            else
            {
                ViewData["IsStudent"] = true;

                return View(await _context.Subject.ToListAsync());
            }
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
                .SingleOrDefaultAsync(m => m.SubjectId == id);
            
            if(subject == null)
                return NotFound();

            if(subject.Lecturer != null)
                subject.Lecturer.FirstName = $"{subject.Lecturer?.FirstName} {subject.Lecturer?.LastName}";
            
            var user = await _userManager.GetUserAsync(this.User);

            if (user != null)
            {
                var userId = await _userManager.GetUserIdAsync(user);

                var student = await _context.Student.FirstOrDefaultAsync(s => s.AccountId == userId);

                if (student == null)
                    return View(subject);

                var exists = await _context.EnrolledSubject
                    .FirstOrDefaultAsync(s => s.SubjectId == id && s.StudentId == student.StudentId);

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
            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code");
            ViewData["CourseId"] = new SelectList(_context.Course.Select(u =>
                        new SelectListItem() { Value = u.CourseId.ToString(), Text = $"{u.Name} ({u.Code})" }), "Value", "Text");


            //ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName");
            ViewData["LecturerId"] = new SelectList(_context.Lecturer.Select(u =>
                        new SelectListItem() { Value = u.LecturerId.ToString(), Text = $"{u.FirstName} {u.LastName}" }), "Value", "Text");

            return View();
        }

        // POST: Subject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,Description,CourseId,LecturerId")] Subject subject)
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

            ViewData["CourseId"] = new SelectList(_context.Course.Select(u =>
                        new SelectListItem() { Value = u.CourseId.ToString(), Text = $"{u.Name} ({u.Code})" }), "Value", "Text");

            ViewData["LecturerId"] = new SelectList(_context.Lecturer.Select(u =>
                        new SelectListItem() { Value = u.LecturerId.ToString(), Text = $"{u.FirstName} {u.LastName}" }), "Value", "Text");

            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code", subject.CourseId);
            //ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", subject.LecturerId);

            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code");
            //ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName");
            return View(subject);
        }

        // GET: Subject/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject.SingleOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            ViewData["CourseId"] = new SelectList(_context.Course.Select(u =>
                        new SelectListItem() { Value = u.CourseId.ToString(), Text = $"{u.Name} ({u.Code})" }), "Value", "Text");

            ViewData["LecturerId"] = new SelectList(_context.Lecturer.Select(u =>
                        new SelectListItem() { Value = u.LecturerId.ToString(), Text = $"{u.FirstName} {u.LastName}" }), "Value", "Text");

            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code", subject.CourseId);
            //ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", subject.LecturerId);
            return View(subject);
        }

        // POST: Subject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,Description,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt,CourseId,LecturerId")] Subject subject)
        {
            if (id != subject.SubjectId)
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
                    if (!SubjectExists(subject.SubjectId))
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

            ViewData["CourseId"] = new SelectList(_context.Course.Select(u =>
                        new SelectListItem() { Value = u.CourseId.ToString(), Text = $"{u.Name} ({u.Code})" }), "Value", "Text");

            ViewData["LecturerId"] = new SelectList(_context.Lecturer.Select(u =>
                        new SelectListItem() { Value = u.LecturerId.ToString(), Text = $"{u.FirstName} {u.LastName}" }), "Value", "Text");

            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Code", subject.CourseId);
            //ViewData["LecturerId"] = new SelectList(_context.Lecturer, "Id", "FirstName", subject.LecturerId);
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
                .SingleOrDefaultAsync(m => m.SubjectId == id);
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
            var subject = await _context.Subject.SingleOrDefaultAsync(m => m.SubjectId == id);
            //_context.Subject.Remove(subject);            
            subject.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subject.Any(e => e.SubjectId == id);
        }

        public string GetMatchingSubjects(string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                var subjects = _context.Subject.Take(10).Select(s => new
                {
                    id = s.SubjectId,
                    text = $"{s.Name} ({s.Code})"
                }).ToList();
                return JsonConvert.SerializeObject(subjects);
            }

            var filtered = _context.Subject.Take(5).Where(s => s.Name.ToLowerInvariant().Contains(searchKey.ToLowerInvariant())).Select(s => new
            {
                id = s.SubjectId,
                text = $"{s.Name} ({s.Code})"
            }).ToList();
            return JsonConvert.SerializeObject(filtered);
        }

        [EnableCors("AllowAllHeaders")]
        public string GetAllSubjects()
        {
            SelectList subjects = new SelectList(_context.Subject.Select(u => new SelectListItem() 
                                                { Value = u.CourseId.ToString(), Text = $"{u.Name} ({u.Code})" }),
                                                "Value", "Text");

            return JsonConvert.SerializeObject(subjects);
        }
    }    
}
