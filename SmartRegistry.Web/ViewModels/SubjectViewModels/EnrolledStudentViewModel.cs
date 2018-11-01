using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRegistry.Web.ViewModels.SubjectViewModels
{
    public class EnrolledStudentViewModel
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        public long StudentNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
    }
}
