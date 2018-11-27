using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.Controllers
{
    public class RegisterController : Controller
    {

        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Register
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Register/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Register/AddNewSensor
        public string AddNewSensor(int sensorId = 0/*, int studentId = 0*/)
        {
            if (sensorId <= 0 /*|| studentId <= 0*/) return JsonConvert.SerializeObject(new {success = false});

            //var student = _context.Student.FirstOrDefault(s => s.Id == studentId);
            //if (student == null) return JsonConvert.SerializeObject(new { success = false });

            //student.SensorId = sensorId;

            var sensor = new Sensor()
            {
                Id = sensorId,
                IsAssigned = false,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            _context.SaveChanges();
            return JsonConvert.SerializeObject(new {success = true});
        }

        [HttpGet]
        public async Task<IActionResult> RegisterUser(int sensorId = 0)
        {

            try
            {
                if (sensorId <= 0)
                    return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Sensor ID must be greater than 0"
                    }));


                //  Log the possible error as hardware shouldn't have done the REQUEST

                var student = await _context.Student.FirstOrDefaultAsync(s => s.SensorId == sensorId);
                if (student == null)
                    return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "No matching student found with sensor ID"
                    }));

                var currentDate = DateTime.UtcNow;
                var schedule = await _context.Schedule.FirstOrDefaultAsync(s => s.ScheduleFor.AddHours(2.0) <= currentDate && s.ScheduleTo.AddHours(2.0) >= currentDate);

                if (schedule == null)
                    return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "No ongoing lecture session found"
                    }));

                var isEnrolled = await _context.EnrolledSubject.FirstOrDefaultAsync(es => es.SubjectId == schedule.SubjectId && es.StudentId == student.Id);

                if (isEnrolled == null)
                    return StatusCode((int)HttpStatusCode.OK, JsonConvert.SerializeObject(new
                    {
                        success = false,
                        fullName = $"{student.FirstName} {student.LastName}",
                        studentNumber = $"{student.StudentNumber.ToString()}",
                        message = "Not Enrolled for Subject"
                    }));


                var attendee = new Attended
                {
                    CreatedAt = DateTime.UtcNow,
                    StudentId = student.Id,
                    ScheduleId = schedule.Id,
                    HasAttended = true
                };

                _context.Attendee.Add(attendee);
                _context.SaveChanges();

                return StatusCode((int)HttpStatusCode.OK, JsonConvert.SerializeObject(new
                {
                    success = true,
                    fullName = $"{student.FirstName} {student.LastName}",
                    studentNumber = $"{student.StudentNumber.ToString()}"
                }));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error: Attendee", ex.Message);

                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }
        }

        

        public async Task<string> MarkRegister(int studentId)
        {
            var student = _context.Student.FirstOrDefault(s => s.Id == studentId);
            if(student == null) return JsonConvert.SerializeObject(new { success = false });

            //  Determine which schedule to mark as to have been attended, thus getting which Subject has been attended
            var schedule = _context.Schedule.FirstOrDefault(s => s.IsConfirmed && s.ScheduleFor.AddHours(2.0) <= DateTime.UtcNow.AddHours(2.0) && s.ScheduleTo.AddHours(2.0) >= DateTime.UtcNow.AddHours(2.0));
            if (schedule == null) return JsonConvert.SerializeObject(new { success = false });

            var attendee = new Attended
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = student.AccountId,
                HasAttended = true,
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = student.AccountId,
                ScheduleId = schedule.Id,
                StudentId = student.Id
            };

            await _context.SaveChangesAsync();

            return JsonConvert.SerializeObject(new { success = true });
        }

        // GET: Register/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Register/Create
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

        // GET: Register/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: Register/Edit/5
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

        // GET: Register/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Register/Delete/5
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

        //[HttpGet]
        //public string RegisterUser(int sensorId = 0) {

        //    if (sensorId <= 0) return JsonConvert.SerializeObject(new { success = false});
        //    //  Log the possible error as hardware shouldn't have done the REQUEST

        //    var student = _context.Student.FirstOrDefault(s => s.SensorId == sensorId);
        //    if(student == null) return JsonConvert.SerializeObject(new { success = false });

        //    var currentDate = DateTime.UtcNow;
        //    var schedule = _context.Schedule.FirstOrDefault(s => s.ScheduleFor <= currentDate && s.ScheduleTo >= currentDate);

        //    if(schedule == null) return JsonConvert.SerializeObject(new { success = false });

        //    var attendee = new Attended
        //    {
        //        CreatedAt = DateTime.UtcNow,
        //        StudentId = student.Id,
        //        ScheduleId = schedule.Id,
        //        HasAttended = true                
        //    };

        //    _context.Attendee.Add(attendee);
        //    _context.SaveChanges();

        //    JsonConvert.SerializeObject(new { success = true });

        //    return JsonConvert.SerializeObject(new { success = false });
        //}
    }
}