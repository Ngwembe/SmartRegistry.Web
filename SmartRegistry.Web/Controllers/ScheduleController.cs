using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartRegistry.Web.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ScheduleController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [EnableCors("AllowAllHeaders")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id < 0)
                return StatusCode((int)HttpStatusCode.Forbidden, "Oops! You have to login first!");

            List<Schedule> schedules = new List<Schedule>()
            {
                new Schedule()
                {
                    ScheduleId = 1,
                    LectureRoom = "10-G230",
                    ColorTheme = "Red",
                    IsConfirmed = true,
                    ScheduleFor = new DateTime(2020, 05, 15, 08,00,00),
                    ScheduleTo = new DateTime(2020, 05, 15, 09,30,00),
                    SubjectId = 1,
                    Subject = new Subject()
                    {
                        SubjectId = 1,
                        Name = "Intelligent Industrial System",
                        Code = "IIS30BT",
                        Description = "Helps students understand robotics ...",
                        CourseId = 1
                    }
                },
                new Schedule()
                {
                    ScheduleId = 2,
                    LectureRoom = "10-G28",
                    ColorTheme = "Green",
                    IsConfirmed = true,
                    ScheduleFor = new DateTime(2020, 05, 16, 08,00,00),
                    ScheduleTo = new DateTime(2020, 05, 16, 09,30,00),
                    SubjectId = 2,
                    Subject = new Subject()
                    {
                        SubjectId = 2,
                        Name = "Discrete Structure IIIA",
                        Code = "DIC301A",
                        Description = "Helps students understand discrete mathematics ...",
                        CourseId = 1
                    }
                },
                new Schedule()
                {
                    ScheduleId = 3,
                    LectureRoom = "10-G270",
                    ColorTheme = "Lime",
                    IsConfirmed = true,
                    ScheduleFor = new DateTime(2020, 05, 17, 08,00,00),
                    ScheduleTo = new DateTime(2020, 05, 17, 09,30,00),
                    SubjectId = 3,
                    Subject = new Subject()
                    {
                        SubjectId = 3,
                        Name = "Technical Programming IIIA",
                        Code = "TPG111A",
                        Description = "Helps students understand programming ...",
                        CourseId = 1
                    }
                },
                new Schedule()
                {
                    ScheduleId = 4,
                    LectureRoom = "10-G220",
                    ColorTheme = "Orange",
                    IsConfirmed = true,
                    ScheduleFor = new DateTime(2020, 05, 17, 11,00,00),
                    ScheduleTo = new DateTime(2020, 05, 17, 15,30,00),
                    SubjectId = 4,
                    Subject = new Subject()
                    {
                        SubjectId = 4,
                        Name = "IT Electronics IIIB",
                        Code = "ITE301B",
                        Description = "Helps students understand electronics ...",
                        CourseId = 1
                    }
                },
                new Schedule()
                {
                    ScheduleId = 5,
                    LectureRoom = "10-G270",
                    ColorTheme = "Lime",
                    IsConfirmed = true,
                    ScheduleFor = new DateTime(2020, 05, 18, 09,30,00),
                    ScheduleTo = new DateTime(2020, 05, 18, 15,30,00),
                    SubjectId = 3,
                    Subject = new Subject()
                    {
                        SubjectId = 3,
                        Name = "Technical Programming IIIA",
                        Code = "TPG111A",
                        Description = "Helps students understand programming ...",
                        CourseId = 1
                    }
                },
				new Schedule()
                {
                    ScheduleId = 6,
                    LectureRoom = "10-G230",
                    ColorTheme = "Red",
                    IsConfirmed = true,
                    ScheduleFor = new DateTime(2020, 06, 16, 08,00,00),
                    ScheduleTo = new DateTime(2020, 06, 16, 09,30,00),
                    SubjectId = 1,
                    Subject = new Subject()
                    {
                        SubjectId = 1,
                        Name = "Intelligent Industrial System",
                        Code = "IIS30BT",
                        Description = "Helps students understand robotics ...",
                        CourseId = 1
                    }
                },
				new Schedule()
                {
                    ScheduleId = 7,
                    LectureRoom = "10-G230",
                    ColorTheme = "Red",
                    IsConfirmed = true,
                    ScheduleFor = new DateTime(2020, 06, 17, 08,00,00),
                    ScheduleTo = new DateTime(2020, 06, 17, 15,30,00),
                    SubjectId = 1,
                    Subject = new Subject()
                    {
                        SubjectId = 1,
                        Name = "Intelligent Industrial System",
                        Code = "IIS30BT",
                        Description = "Helps students understand robotics ...",
                        CourseId = 1
                    }
                }
            };

            //List<Schedule> results = schedules.Where(s => s.SubjectId == id).ToList();

            return new OkObjectResult(_context.Schedule.Where(s => s.SubjectId == id).Include(s => s.Subject).ToList());
            //return new OkObjectResult(JsonConvert.SerializeObject(results));
        }

        [EnableCors("AllowAllHeaders")]
        [HttpGet("{userId}/{isStudent}")]
        public async Task<IActionResult> Get(int userId, bool isStudent)
        {
            if (userId < 0)
                return StatusCode((int)HttpStatusCode.Forbidden, "Oops! You have to login first!");

            var student = _context.Student.FirstOrDefault(s => s.StudentId == userId);

            if (student != null && isStudent)
            {
                List<EnrolledSubject> enrolledSubjects = await _context.EnrolledSubject
                    .Where(e => e.StudentId == student.StudentId)
                    .ToListAsync();

                var schedules = new List<object>();

                foreach (var enrolledSubject in enrolledSubjects)
                {
                    schedules.AddRange(_context.Schedule
                        .Where(s => s.SubjectId == enrolledSubject.SubjectId)
                        .Include(s => s.Subject)
                        .Select(s => new
                        {
                            Schedule = new {
                                s.ScheduleId,
                                s.ScheduleFor,
                                s.ScheduleTo,
                                s.LectureRoom,
                                s.IsConfirmed
                            },
                            Subject = new
                            {
                                s.SubjectId,
                                s.Subject.Name,
                                s.Subject.Code
                            }
                        }));
                }

                return new OkObjectResult(schedules.ToList());
            }
            
            var lecturer = _context.Lecturer.FirstOrDefault(s => s.LecturerId == userId);

            if (lecturer != null && !isStudent)
            {
                List<Subject> enrolledSubjects = await _context.Subject
                    .Where(e => e.LecturerId == lecturer.LecturerId)
                    .ToListAsync();

                var schedules = new List<Schedule>();

                foreach (var enrolledSubject in enrolledSubjects)
                {
                    schedules.AddRange(_context.Schedule.Where(s => s.SubjectId == enrolledSubject.SubjectId).Include(s => s.Subject));
                }

                return new OkObjectResult(schedules.ToList());
            }
            
            return new BadRequestResult();
            //return new OkObjectResult(JsonConvert.SerializeObject(results));
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
