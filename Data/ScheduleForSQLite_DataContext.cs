using dotnetcore_desktop_app.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore_desktop_app.Data
{
    public class ScheduleForSQLite_DataContext : DbContext
    {
        public ScheduleForSQLite_DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<ScheduleForSQLite> ScheduleForSQLites => Set<ScheduleForSQLite>();
    }
}
