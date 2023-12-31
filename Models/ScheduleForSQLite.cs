namespace dotnetcore_desktop_app.Models
{
    public class ScheduleForSQLite
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string cName { get; set; }
        public string employeeId { get; set; }
        public string WorkShift { get; set; }
    }
}
