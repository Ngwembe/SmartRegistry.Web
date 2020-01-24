using SmartRegistry.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRegistry.Web.ViewModels.SchedulesViewModels
{
    public class ScheduleDetailsViewModel
    {
        public int ScheduleId { get; set; }

        public string LectureRoom { get; set; }

        public bool IsConfirmed { get; set; }

        public DateTime ScheduleFor { get; set; }

        public DateTime ScheduleTo { get; set; }

        public string ColorTheme { get; set; }
        
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public float Present { get; set; }
        public float Absent { get; set; }
    }
}
