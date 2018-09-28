using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.Data.Service
{
    public interface IAttendanceService
    {
        Student AddStudent(Student student);
        Lecturer AddLecturer(Lecturer lecturer);
    }
}
