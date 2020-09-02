using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;
using SmartRegistry.Web.Hubs;
using SmartRegistry.Web.ViewModels.HomeViewModels;

namespace SmartRegistry.Web.Controllers
{
    public class RegisterController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        ///<summary></summary>
        public RegisterController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        // GET: Register
        public ActionResult Index()
        {
           return View();
        }

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
                SensorId = sensorId,
                IsAssigned = false,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            _context.SaveChanges();
            return JsonConvert.SerializeObject(new {success = true});
        }

        public IActionResult GetSubjects()
        {
            var faculties = _context.Faculty.ToList();
            var announcements = _context.Announcement.Where(a => a.AnnouncementTypeId == 1).ToList();

            var homepageVM = new HomePageViewModel()
            {
                Announcements = announcements,
                Faculties = faculties,
                Description = "This is institution's brief description... () To be pulled from the DB"
            };

            ViewData["HostName"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            return PartialView("_GetSubjectsPartial", homepageVM);
        }

        //[EnableCors("AllowAllHeaders")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]AuthParams authParams)
        {
            try
            {
                if (authParams.SensorId <= 0)
                    return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Sensor ID must be greater than 0"
                    }));

                if (authParams.ScheduleId <= 0)
                    return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "Schedule ID must be greater than 0"
                    }));

                var student = await _context.Student.FirstOrDefaultAsync(s => s.SensorId == authParams.SensorId);
                if (student == null)
                    return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "No matching student found with sensor ID"
                    }));

                var currentDate = DateTime.UtcNow;
                var schedule = await _context.Schedule.FirstOrDefaultAsync(s => s.ScheduleId == authParams.ScheduleId);

                if (schedule == null)
                    return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
                    {
                        success = false,
                        message = "No ongoing lecture session found"
                    }));

                var isEnrolled = await _context.EnrolledSubject.FirstOrDefaultAsync(es => es.SubjectId == schedule.SubjectId && es.StudentId == student.StudentId);

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
                    StudentId = student.StudentId,
                    ScheduleId = schedule.ScheduleId,
                    HasAttended = true
                };

                _context.Attendee.Add(attendee);
                _context.SaveChanges();

                MessageExchange broadcast = new MessageExchange()
                {
                    Message = $"{student?.FirstName} {student?.LastName} has been registered successfully ...",
                    ReceivedAt = DateTime.Now.ToString("g"),
                    Sender = "SmartWatcher"
                };

                //await _hubContext.Clients.All.SendAsync("SendMessageToAll", JsonConvert.SerializeObject(broadcast));
                //Console.WriteLine("Suppose to notify all users");

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

        //[HttpGet]
        //public async Task<IActionResult> RegisterUser(int sensorId = 0)
        //{
        //    try
        //    {
        //        if (sensorId <= 0)
        //            return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
        //            {
        //                success = false,
        //                message = "Sensor ID must be greater than 0"
        //            }));

        //        MessageExchange broadcast = new MessageExchange()
        //        {
        //            Message = "Tsepo has been registered successfully ...",
        //            ReceivedAt = DateTime.Now.ToString("g"),
        //            Sender = "System"
        //        };

        //        await _hubContext.Clients.All.SendAsync("OnlineReceiveMessage", JsonConvert.SerializeObject(broadcast));
        //        Console.WriteLine("Suppose to notify all users");

        //        // if(sensorId == 1) {                    
        //            // MessageExchange broadcast = new MessageExchange()
        //            // {
        //                // Message = "Tsepo has been registered successfully ...",
        //                // ReceivedAt = DateTime.Now.ToString("g"),
        //                // Sender = "System"
        //            // };

        //            // await _hubContext.Clients.All.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(broadcast));
        //            // Console.WriteLine("Suppose to notify all users");
        //        // }

        //        //  Log the possible error as hardware shouldn't have done the REQUEST

        //        var student = await _context.Student.FirstOrDefaultAsync(s => s.SensorId == sensorId);
        //        if (student == null)
        //            return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
        //            {
        //                success = false,
        //                message = "No matching student found with sensor ID"
        //            }));

        //        var currentDate = DateTime.UtcNow;
        //        var schedule = await _context.Schedule.FirstOrDefaultAsync(s => s.ScheduleFor.AddHours(2.0) <= currentDate && s.ScheduleTo.AddHours(2.0) >= currentDate);

        //        if (schedule == null)
        //            return StatusCode((int)HttpStatusCode.NotFound, JsonConvert.SerializeObject(new
        //            {
        //                success = false,
        //                message = "No ongoing lecture session found"
        //            }));

        //        var isEnrolled = await _context.EnrolledSubject.FirstOrDefaultAsync(es => es.SubjectId == schedule.SubjectId && es.StudentId == student.StudentId);

        //        if (isEnrolled == null)
        //            return StatusCode((int)HttpStatusCode.OK, JsonConvert.SerializeObject(new
        //            {
        //                success = false,
        //                fullName = $"{student.FirstName} {student.LastName}",
        //                studentNumber = $"{student.StudentNumber.ToString()}",
        //                message = "Not Enrolled for Subject"
        //            }));


        //        var attendee = new Attended
        //        {
        //            CreatedAt = DateTime.UtcNow,
        //            StudentId = student.StudentId,
        //            ScheduleId = schedule.ScheduleId,
        //            HasAttended = true
        //        };

        //        _context.Attendee.Add(attendee);
        //        _context.SaveChanges();

        //        return StatusCode((int)HttpStatusCode.OK, JsonConvert.SerializeObject(new
        //        {
        //            success = true,
        //            fullName = $"{student.FirstName} {student.LastName}",
        //            studentNumber = $"{student.StudentNumber.ToString()}"
        //        }));
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("Error: Attendee", ex.Message);

        //        return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
        //    }
        //}

        public async Task<string> MarkRegister(int studentId)
        {
            var student = _context.Student.FirstOrDefault(s => s.StudentId == studentId);
            if(student == null) return JsonConvert.SerializeObject(new { success = false });

            var currentDateTime = DateTime.UtcNow;

            //  Determine which schedule to mark as to have been attended, thus getting which Subject has been attended
            var schedule = _context.Schedule.FirstOrDefault(s => s.IsConfirmed && s.ScheduleFor//.AddHours(2.0) 
                                                              <= currentDateTime.AddHours(2.0) 
                                                              &&
                                                            s.ScheduleTo//.AddHours(2.0) 
                                                            >= currentDateTime.AddHours(2.0).AddMinutes(30) //currentDateTime.AddHours(2.0)
                                                            );
            if (schedule == null) 
                return JsonConvert.SerializeObject(new { success = false });

            var attendee = new Attended
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = student.AccountId,
                HasAttended = true,
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = student.AccountId,
                ScheduleId = schedule.ScheduleId,
                StudentId = student.StudentId
            };

            _context.Add(attendee);
            await _context.SaveChangesAsync();
            //await _context.SaveChangesAsync();

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

    public class AuthParams
    {
        public int SensorId { get; set; }
        public int ScheduleId { get; set; }
    }
}