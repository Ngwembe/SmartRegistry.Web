using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.Data.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AttendanceService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Student AddStudent(Student student)
        {
            _applicationDbContext.Student.Add(student);
            _applicationDbContext.SaveChanges();

            return student;
        }

        public Lecturer AddLecturer(Lecturer lecturer)
        {
            _applicationDbContext.Lecturer.Add(lecturer);
            _applicationDbContext.SaveChanges();

            return lecturer;
        }
    }
}
