using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartRegistry.Web.Migrations
{
    public partial class PrefixingIdsForEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Subject",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Student",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Sensor",
                newName: "SensorId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Schedule",
                newName: "ScheduleId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Lecturer",
                newName: "LecturerId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Faculty",
                newName: "FacultyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EnrolledSubject",
                newName: "EnrolledSubjectId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Department",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Course",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Contact",
                newName: "ContactId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Attendee",
                newName: "AttendedId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Announcement",
                newName: "AnnouncementId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Address",
                newName: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Subject",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Student",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SensorId",
                table: "Sensor",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "Schedule",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LecturerId",
                table: "Lecturer",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Faculty",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EnrolledSubjectId",
                table: "EnrolledSubject",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Department",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Course",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ContactId",
                table: "Contact",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AttendedId",
                table: "Attendee",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AnnouncementId",
                table: "Announcement",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Address",
                newName: "Id");
        }
    }
}
