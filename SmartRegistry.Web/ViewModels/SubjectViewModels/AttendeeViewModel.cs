using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRegistry.Web.ViewModels.SubjectViewModels
{
    public class AttendeeViewModel
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        public long StudentNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }

        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }

        public DateTime ScheduleFrom { get; set; }
        public DateTime ScheduleTo { get; set; }
    }
}
