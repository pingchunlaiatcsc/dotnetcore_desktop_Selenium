using dotnetcore_desktop_app.Models;
using prjC349WebMVC.Library.WebCrawler;

namespace dotnetcore_desktop_app.ViewModels
{
    public class ScheduleViewModel
    {
        public List<ScheduleModel> schedules {  get; set; }

        public List<DateTime> dates { get; set; }
    }
}
