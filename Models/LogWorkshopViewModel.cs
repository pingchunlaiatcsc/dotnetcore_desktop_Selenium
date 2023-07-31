using EIPLibrary.WebCrawler;

namespace dotnetcore_desktop_app.Models
{
    public class LogWorkshopViewModel
    {
        public string userId { get; set; }
        public string userPassword { get; set; }
        public LogWorkshop.Model model { get; set; }
        public List<string> DutyOfficers { get; set; }
    }
}
