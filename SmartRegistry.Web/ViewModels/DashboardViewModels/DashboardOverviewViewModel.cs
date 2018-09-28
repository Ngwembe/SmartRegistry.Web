namespace SmartRegistry.Web.ViewModels.DashboardViewModels
{
    public class DashboardOverviewViewModel
    {
        public string AllSchedules { get; set; }

        public int Attended { get; set; }
        public int Missed { get; set; }
        public int Confirmed { get; set; }
        public int Unconfirmed { get; set; }
    }

}
