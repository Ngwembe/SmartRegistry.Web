using SmartRegistry.Web.Models;
using System;

namespace SmartRegistry.Web.ViewModels.SchedulesViewModels
{
    public class MainScheduleViewModel
    {
        public Schedule Schedule { get; set; }
    }

    public class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public float Present { get; set; }
        public float Absent { get; set; }

        public DateTime Date { get; set; }
    }
}
