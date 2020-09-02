﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;
using SmartRegistry.Web.ViewModels.SchedulesViewModels;

namespace SmartRegistry.Web.Controllers
{
    [Authorize(Roles = "System Admin,Admin,Lecturer")]
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SchedulesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Schedule
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            List<Schedule> schedules = null;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                schedules = _context.Schedule.Include(s => s.Subject).Where(s => s.CreatedBy == userId && !s.IsDeleted).ToList();
                return View(schedules.ToList());
            }

            schedules = _context.Schedule.Include(s => s.Subject).Where(s => !s.IsDeleted).ToList();

            //if(schedules.Any())
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Code");
            return View(schedules);
        }

        //public IActionResult Index(string userId)
        //{
        //    var user = _userManager.FindByIdAsync(userId);

        //    if (user == null)
        //        return StatusCode((int) HttpStatusCode.Forbidden, "Oops! You have to login first!");

        //    List<Schedule> schedules = null;

        //    if (!string.IsNullOrWhiteSpace(userId))
        //    {
        //        schedules = _context.Schedule.Include(s => s.Subject).Where(s => s.CreatedBy == userId && !s.IsDeleted).ToList();
        //        return View(schedules.ToList());
        //    }

        //    schedules = _context.Schedule.Include(s => s.Subject).Where(s => !s.IsDeleted).ToList();

        //    return new OkObjectResult(JsonConvert.SerializeObject(schedules));
        //}

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Subject)
                .SingleOrDefaultAsync(m => m.ScheduleId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedule/Create
        public IActionResult Create()
        {
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Code");
            return View();
        }

        public IActionResult Create(Schedule schedule, bool isCreated)
        {
            if (schedule == null)
            {
                ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Code");
                return View();
            }


            //ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Code");
            
            return View(schedule);
        }

        public JsonResult GetSchedules()
        {
            //var schedules = _context.Schedule.Include(s => s.Subject)
            //                                  .Where(s => !s.IsDeleted)
            //                                  .Select(s => new {
            //                                      Id = s.Id,
            //                                      Subject = s.Subject,
            //                                      ScheduleFor = s.ScheduleFor.ToString("dd-MM-yyyy HH:mm:ss"),
            //                                      ScheduleTo = s.ScheduleTo.ToString("dd-MM-yyyy HH:mm:ss"),
            //                                      LectureRoom = s.LectureRoom,
            //                                      IsConfirmed = s.IsConfirmed
            //                                    });

            var schedules = _context.Schedule.Include(s => s.Subject)
                .Where(s => !s.IsDeleted)
                .Select(s => new {
                    Id = s.ScheduleId,
                    Subject = s.Subject,
                    ScheduleFor = s.ScheduleFor, //.ToString("dd-MM-yyyy HH:mm:ss a"),
                    ScheduleTo = s.ScheduleTo, //.ToString("dd-MM-yyyy HH:mm:ss a"),
                    LectureRoom = s.LectureRoom,
                    IsConfirmed = s.IsConfirmed
                });

            return new JsonResult(schedules);

            //return new JsonResult { Data = schedules.ToList(), JsonResquestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult PullSchedules(int subjectId)
        {
            var schedules = _context.Schedule
                .Include(s => s.Subject)
                .Where(s => s.SubjectId == subjectId)
                .Select(s => new {
                    Id = s.ScheduleId,
                    Subject = s.Subject,
                    ScheduleFor = s.ScheduleFor, //.ToString("dd-MM-yyyy HH:mm:ss a"),
                    ScheduleTo = s.ScheduleTo, //.ToString("dd-MM-yyyy HH:mm:ss a"),
                    LectureRoom = s.LectureRoom,
                    IsConfirmed = s.IsConfirmed
                    //,Attended = _context.Attendee.Where(a => a.ScheduleId == s.ScheduleId).Count(),
                    //Absent = _context.EnrolledSubject.Count(e => e.SubjectId == s.SubjectId) - _context.Attendee.Where(a => a.ScheduleId == s.ScheduleId).Count()
                });

            return new JsonResult(schedules);
        }

        public async Task<JsonResult> PullSchedule(int scheduleId)
        {
            var schedule = await _context.Schedule.Include(s => s.Subject).FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null) return new JsonResult(null);

            var total = await _context.EnrolledSubject.CountAsync(e => e.SubjectId == schedule.Subject.SubjectId);
            var present = await _context.Attendee.Where(a => a.ScheduleId == schedule.ScheduleId).CountAsync();
            var absent = total - present;

            var details = MapperHelper.Map(new ScheduleDetailsViewModel(), schedule);
            bool v = total > 0;

            if(v)
            {
                details.Present = ((float)present / (float)total) * 100.0f;                
                details.Absent = ((float)absent / (float)total) * 100.0f;
            }
            else
            {
                details.Present = present * 100.0f;
                details.Absent = absent * 100.0f;
            }                      

            //var schedules = _context.Schedule
            //    .Include(s => s.Subject)
            //    .Where(s => s.ScheduleId == scheduleId)
            //    .Select(s => new {
            //        Id = s.ScheduleId,
            //        Subject = s.Subject,
            //        ScheduleFor = s.ScheduleFor, //.ToString("dd-MM-yyyy HH:mm:ss a"),
            //        ScheduleTo = s.ScheduleTo, //.ToString("dd-MM-yyyy HH:mm:ss a"),
            //        LectureRoom = s.LectureRoom,
            //        IsConfirmed = s.IsConfirmed
            //        //,Attended = _context.Attendee.Where(a => a.ScheduleId == s.ScheduleId).Count(),
            //        //Absent = _context.EnrolledSubject.Count(e => e.SubjectId == s.SubjectId) - _context.Attendee.Where(a => a.ScheduleId == s.ScheduleId).Count()
            //    });

            return new JsonResult(details);
        }

        public async Task<JsonResult> RetrieveSchedules(int subjectId)
        {
            var schedules = await _context.Schedule.Include(s => s.Subject)
                                                   .Where(s => s.SubjectId == subjectId)
                                                   //.GroupBy(s => s.ScheduleFor.Date)
                                                   .ToListAsync();
            //var schedules = await _context.Schedule.Include(s => s.Subject).Where(s => s.SubjectId == subjectId).ToListAsync();

            if (schedules == null || !schedules.Any()) return new JsonResult(null);

            var totalEnrollments = await _context.EnrolledSubject.CountAsync(e => e.SubjectId == schedules.FirstOrDefault().Subject.SubjectId);

            float present = 0f, absent = 0f;

            var scheduleData = new List<ScheduleViewModel>();

            schedules.ForEach(async s => {

                present =  _context.Attendee.Count(a => a.ScheduleId == s.ScheduleId);
                absent = totalEnrollments - present;

                scheduleData.Add(new ScheduleViewModel { ScheduleId = s.ScheduleId, Absent = absent, Present = present, Date = s.ScheduleFor });
            });

            return new JsonResult(new { scheduleData = scheduleData.OrderBy(s => s.Date), subject = schedules.FirstOrDefault()?.Subject});

            //var details = MapperHelper.Map(new ScheduleDetailsViewModel(), schedules);
            //bool v = totalEnrollments > 0;

            //if (v)
            //{
            //    details.Present = ((float)present / (float)totalEnrollments) * 100.0f;
            //    details.Absent = ((float)absent / (float)totalEnrollments) * 100.0f;
            //}
            //else
            //{
            //    details.Present = present * 100.0f;
            //    details.Absent = absent * 100.0f;
            //}

            //return new JsonResult(details);
        }

        public async Task<JsonResult> RetrieveMonthly(int subjectId)
        {
            var schedules = await _context.Schedule.Include(s => s.Subject)
                                                   .Where(s => s.SubjectId == subjectId)                                                   
                                                   .OrderBy(s => s.ScheduleFor.Month)
                                                   .GroupBy(s => s.ScheduleFor.Month)
                                                   .ToListAsync();
            //var schedules = await _context.Schedule.Include(s => s.Subject).Where(s => s.SubjectId == subjectId).ToListAsync();

            if (schedules == null || !schedules.Any()) return new JsonResult(null);

            var totalEnrollments = await _context.EnrolledSubject.CountAsync(e => e.SubjectId == schedules[0].FirstOrDefault().Subject.SubjectId);
            //var totalEnrollments = await _context.EnrolledSubject.CountAsync(e => e.SubjectId == schedules.FirstOrDefault().Subject.SubjectId);

            float present = 0f, absent = 0f;

            var scheduleData = new List<ScheduleViewModel>();

            for(int x = 0; x < schedules.Count(); x++)
            {
                present = 0f; absent = 0f;

                var sets = schedules[x].ToList();

                sets.ForEach(async s =>
                {
                    present += Math.Abs(_context.Attendee.Count(a => a.ScheduleId == s.ScheduleId));
                    absent += Math.Abs(totalEnrollments - present);
                });

                scheduleData.Add(new ScheduleViewModel { Absent = absent, Present = present, Date = sets.FirstOrDefault().ScheduleFor.Date });
            }


            //schedules.ForEach(async s => {

            //    present = _context.Attendee.Count(a => a.ScheduleId == s.ScheduleId);
            //    absent = totalEnrollments - present;

            //    scheduleData.Add(new ScheduleViewModel { ScheduleId = s.ScheduleId, Absent = absent, Present = present, Date = s.ScheduleFor });
            //});

            return new JsonResult(scheduleData);            
        }

        public string ConfirmSchedule(int scheduleId)
        {
            var schedule = _context.Schedule.FirstOrDefault(s => s.ScheduleId == scheduleId);

            if(schedule == null) return JsonConvert.SerializeObject(new { success = false });

            schedule.IsConfirmed = true;
            schedule.LastUpdatedAt = DateTime.UtcNow;
            schedule.LastUpdatedBy = _userManager.GetUserId(HttpContext.User) ?? schedule.LastUpdatedBy;

            _context.SaveChanges();

            return JsonConvert.SerializeObject(new { success = true });
        }

        //public IActionResult GetCreateModalPartialView(DateTime? startDate = null, DateTime? endDate = null, bool isAllDay = false)
        //[HttpPost]
        //public IActionResult GetCreateModalPartialView([Bind("startDate,endDate,lectureRoom,ScheduleTo,IsConfirmed")]string startDate, string endDate, string lectureRoom, bool isAllDay = false)
        //{
        //    //var dateToBeCreated = new Schedule()
        //    //{
        //    //    ScheduleFor = (startDate != null) ? Convert.ToDateTime(startDate) : DateTime.UtcNow,
        //    //    ScheduleTo = (endDate != null) ? Convert.ToDateTime(endDate) : DateTime.UtcNow
        //    //};

        //    IFormatProvider culture = new CultureInfo("en-ZA", true); //culture = new CultureInfo("en-US", true);
        //    var format = "dd/MM/yyyy"; //format = "dd/MM/yyyy HH:mm:ss.fff";


        //    var dateToBeCreated = new Schedule()
        //    {

        //        ScheduleFor = DateTime.ParseExact(startDate, format, culture),// (startDate),
        //        ScheduleTo = DateTime.ParseExact(endDate, format, culture)// Convert.ToDateTime(endDate)
        //    };

        //    return View("Create", dateToBeCreated);
        //    //return PartialView("_CreateSchedulePartial", dateToBeCreated);
        //}

        //[HttpPost]

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetCreateModalPartialView([Bind("Id,LectureRoom,ScheduleFor,ScheduleTo,IsConfirmed,SubjectId")] Schedule schedule, string startDate, string endDate, string lectureRoom, bool isAllDay = false)
        {
            //var dateToBeCreated = new Schedule()
            //{
            //    ScheduleFor = (startDate != null) ? Convert.ToDateTime(startDate) : DateTime.UtcNow,
            //    ScheduleTo = (endDate != null) ? Convert.ToDateTime(endDate) : DateTime.UtcNow
            //};

            //IFormatProvider culture = new CultureInfo("en-ZA", true); //culture = new CultureInfo("en-US", true);
            //var format = "dd/MM/yyyy"; //format = "dd/MM/yyyy HH:mm:ss.fff";
            
            //var dateToBeCreated = new Schedule()
            //{
            //    ScheduleFor = DateTime.ParseExact(startDate, format, culture),// (startDate),
            //    ScheduleTo = DateTime.ParseExact(endDate, format, culture)// Convert.ToDateTime(endDate)
            //};

            //return View(schedule);
            //return View("Create", dateToBeCreated);
            
            var user = await _userManager.GetUserAsync(this.User);

            if (user == null) return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Dashboard", new { subjectId = schedule.SubjectId })});

            var userId = await _userManager.GetUserIdAsync(user);

            if (userId == null) return Create();

            var userIdentity = await _userManager.FindByIdAsync(userId);

            if (await _userManager.IsInRoleAsync(userIdentity, "System Admin") || await _userManager.IsInRoleAsync(userIdentity, "Admin"))
            {
                var subjects = await _context.Subject.Include(s => s.Lecturer).ToListAsync();//.AsEnumerable();

                //ViewData["SubjectId"] = new SelectList(subjects, "Id", "Code");
                ViewBag.SubjectId = new SelectList(subjects.Select(u =>
                    new SelectListItem() { Value = u.CourseId.ToString(), Text = $"{u.Name} ({u.Code})" }), "Value", "Text");

                return PartialView("_CreateSchedulePartial", schedule);
            }
            else //if (await _userManager.IsInRoleAsync(userIdentity, "Lecturer"))
            {
                var lecturer = _context.Lecturer.FirstOrDefault(l => l.AccountId == userId);

                if (lecturer == null)
                    return View(nameof(Index));

                var subjects = await _context.Subject.Include(s => s.Lecturer).Where(s => s.Lecturer.LecturerId == lecturer.LecturerId).ToListAsync();//.AsEnumerable();
                
                //ViewData["SubjectId"] = new SelectList(subjects, "Id", "Code");
                ViewBag.SubjectId = new SelectList(subjects.Select(u =>
                    new SelectListItem() { Value = u.CourseId.ToString(), Text = $"{u.Name} ({u.Code})" }), "Value", "Text");

                return PartialView("_CreateSchedulePartial", schedule);
            }
            //else
            //{
            //    return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Dashboard", new { subjectId = schedule.SubjectId }) });
            //}
        }

        // POST: Schedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LectureRoom,ScheduleFor,ScheduleTo,IsConfirmed,SubjectId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                if (schedule.ScheduleFor > schedule.ScheduleTo)
                    return View(schedule);
                
                var userId = _userManager.GetUserId(HttpContext.User);

                schedule.CreatedAt = DateTime.UtcNow;
                schedule.LastUpdatedAt = DateTime.UtcNow;
                schedule.CreatedBy = userId;
                schedule.LastUpdatedBy = userId;

                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Code", schedule.SubjectId);
            return View(schedule);
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.SingleOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            //ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Code", schedule.SubjectId);
            ViewBag.SubjectId = new SelectList(_context.Subject.Select(u =>
                new SelectListItem() { Value = u.CourseId.ToString(), Text = $"{u.Name} ({u.Code})" }), "Value", "Text", schedule.SubjectId);

            return View(schedule);
        }

        // POST: Schedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,LectureRoom,IsConfirmed,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt,SubjectId")] Schedule schedule)
        public async Task<IActionResult> Edit(int scheduleId, [Bind("Id,LectureRoom,IsConfirmed,ScheduleFor, ScheduleTo,SubjectId, ScheduleId")] Schedule schedule)
        {
            //if (scheduleId != schedule.ScheduleId)
            //{
            //    return NotFound();
            //}
            var result = _context.Schedule.FirstOrDefault(s => s.ScheduleId == schedule.ScheduleId);
            //var result = _context.Schedule.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (result == null) return NotFound();

            result.LectureRoom = schedule.LectureRoom;
            result.ScheduleFor = schedule.ScheduleFor;
            result.ScheduleTo = schedule.ScheduleTo;
            result.IsConfirmed = schedule.IsConfirmed;
            result.LastUpdatedAt = DateTime.UtcNow;


            //if (ModelState.IsValid)
            //{
                try
                {
                    //_context.Update(schedule);
                    //await _context.SaveChangesAsync();
                    _context.Update(result);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ScheduleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["SubjectId"] = new SelectList(_context.Subject, "Id", "Code", schedule.SubjectId);
            //return View(schedule);
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Subject)
                .SingleOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedule.SingleOrDefaultAsync(m => m.ScheduleId == id);
            //_context.Schedule.Remove(schedule);
            schedule.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedule.Any(e => e.ScheduleId == id);
        }
    }
}
