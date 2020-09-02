using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;
using SmartRegistry.Web.ViewModels.DashboardViewModels;

namespace SmartRegistry.Web.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Dashboard
        public async Task<IActionResult> Index(int subjectId) //  To be used by Admins
        {
            var user = await _userManager.GetUserAsync(this.User);

            if (user == null) return RedirectToAction("Login", "Account", new { returnUrl  = "Home"});

            var userId = await _userManager.GetUserIdAsync(user);

            if (userId == null) return RedirectToAction("Login", "Account", new { returnUrl = "Home" });

            var userIdentity = await _userManager.FindByIdAsync(userId);

            if (await _userManager.IsInRoleAsync(userIdentity, "System Admin") || await _userManager.IsInRoleAsync(userIdentity, "Admin"))
            {
                var subjects = await _context.Subject.Include(s => s.Lecturer).Where(c => c.SubjectId == subjectId).ToListAsync();

                var startDate = DateTime.Now - DateTime.Now.Subtract(DateTime.Now.AddDays(-30));
                var endDate = DateTime.Now.AddDays(30);

                if (subjects == null) return View(new DashboardModel());

                return View(new DashboardModel() { Subject = subjects.FirstOrDefault(), FilterFromDate = startDate, FilterToDate = endDate });

                //if (lecturer == null) return View(new DashboardModel());
                //if (subjects == null) return View(new List<Student>() as IEnumerable<Subject>);

                //var result = subjects.Select(s => new
                //{
                //    subject = s.Name,
                //    value = 1
                //}).ToList();

                //return View(subjects);
            }
            else
            {
                var lecturer = await _context.Lecturer.FirstOrDefaultAsync(a => a.AccountId == userId);
                if (lecturer == null) return View();

                var subjects = await _context.Subject.Include(s => s.Lecturer).Where(c => c.SubjectId == subjectId && c.LecturerId == lecturer.LecturerId).ToListAsync();

                var startDate = DateTime.Now - DateTime.Now.Subtract(DateTime.Now.AddDays(-30));
                var endDate = DateTime.Now + DateTime.Now.Subtract(DateTime.Now.AddDays(30));

                if (subjects == null) return View(new DashboardModel());

                return View(new DashboardModel() { Subject = subjects.FirstOrDefault(), FilterFromDate = startDate, FilterToDate = endDate });

                //var subjects = await _context.Subject.Include(s => s.Lecturer).Where(c => c.SubjectId == subjectId && c.LecturerId == lecturer.LecturerId).ToListAsync();
                //if (subjects == null) return View(subjects);

                //var result = subjects.Select(s => new
                //{
                //    subject = s.Name,
                //    value = 1
                //}).ToList();

                //return View(subjects);
            }
        }

        public async Task<IActionResult> Statistics()
        {
            return View(new AdminStatsDashboardViewModel(){SiteVisits = 50, TotalStudents = await _context.Student.CountAsync() });
        }

        //GET: Dashboard/Details/5
        public IActionResult Details(int subjectId, int studentId) //  When viewing a student's record
        {
            var a = _context.Attendee.ToList();
            var attendance =(from attendee in _context.Attendee
                                        join schedule in _context.Schedule
                                        on attendee.ScheduleId equals schedule.ScheduleId
                                        where attendee.Schedule.SubjectId == subjectId
                                        select attendee).ToList();
            
            //var enrolledSubject = _context.EnrolledSubject.FirstOrDefault(s => s.StudentId == studentId && s.SubjectId == subjectId);
            //if (enrolledSubject == null)
            //{
            //    ModelState.AddModelError("Subject_Error", "Student not enrolled on the specified subject");
            //    return View(ModelState);
            //}

            //var attendance = _context.Attendee
            //                                    .Include(a => a.Student)
            //                                    .Include(a => a.Schedule)
            //                                    .ThenInclude(a => a.Subject)
            //                                    .Where(a => a.Schedule.SubjectId == subjectId)
            //                                    //.Where(a => a.StudentId == studentId)
            //                                    .ToList();

            if (!attendance.Any())
            {
                ModelState.AddModelError("Subject_Error", "Student not enrolled on the specified subject");
                return View(ModelState);
            }

            var response = attendance.GroupBy(a => a.HasAttended)
                                                   .ToList();

            return View();
        }

        public string GetDailyStats(int scheduleId)
        {            
            try
            {
                var subject = _context.Schedule.Include(s => s.Subject).FirstOrDefault(s => s.ScheduleId == scheduleId)?.Subject;                
                if (subject == null) return JsonConvert.SerializeObject(new { success = false });

                //return JsonConvert.SerializeObject(new
                //{
                //    a = new {
                //        label = "attended", value = _context.Attendee.Count(s => s.ScheduleId == scheduleId)
                //    }
                //    ,
                //    b = new
                //    {
                //        label = "unattended",
                //        value = _context.EnrolledSubject.Count(s => s.SubjectId == subject.Id)
                //    }
                //});

                var attended = new
                {
                    label = "attended",
                    value = 30//_context.Attendee.Count(s => s.ScheduleId == scheduleId)
                };

                var unattended = new
                {
                    label = "unattended",
                    value = 20//_context.EnrolledSubject.Include(en => en.Subject).Count(s => s.SubjectId == subject.Id) - attended.value
                };

                return JsonConvert.SerializeObject(new
                {
                    attended,
                    unattended,
                    subjectName = $"{subject.Name} ({subject.Code})"
                });

                //return JsonConvert.SerializeObject( new
                //{
                //    [
                //        new { attended = _context.Attendee.Count(s => s.ScheduleId == scheduleId) },
                //        new { unattended = _context.EnrolledSubject.Include(en => en.Subject).Count(s => s.SubjectId == subject.Id) - attended.value }
                //    ]
                //});

            }
            catch (Exception ex) {
                return JsonConvert.SerializeObject(new { success = false });
            }           
        }

        //  Need to determine all the schedules of a week the current date amd time and calculate the attendance rate from there
        //  POSSIBLE SOLUTION: Have two calendar date controls to choose from the week dates and use that as range
        public async Task<string> GetWeeklyStats(int subjectId)
        {
            var schedules = await _context.Schedule
                                    .Include(s => s.Subject)
                                    .Where(s => s.IsConfirmed && s.SubjectId == subjectId && s.ScheduleTo.Date <= DateTime.UtcNow.Date).ToListAsync();
        
            var result = schedules.Select(async s =>
            {
                var totalCount = await _context.Attendee/*.Where(a => a.ScheduleId == s.Id && a.HasAttended)*/
                                               .CountAsync(a => a.ScheduleId == s.ScheduleId && a.HasAttended);
                return
                new
                {
                    value = totalCount
                };
            });//.ToAsyncEnumerable();

            return JsonConvert.SerializeObject(result);
        }

        // GET: Dashboard/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Dashboard/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Dashboard/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: Dashboard/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Dashboard/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Dashboard/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }

    public class DashboardModel
    {
        public Subject Subject { get; set; }
        
        [DisplayName("Filter From Date")]
        public DateTime FilterFromDate { get; set; }
        [DisplayName("Filter To Date")]
        public DateTime FilterToDate { get; set; }
    }
}