using dotnetcore_desktop_app.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore_desktop_app.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        public DbSet<RpgCharater> RpgCharaters => Set<RpgCharater>();

        public DbSet<dotnetcore_desktop_app.Models.C341TaskSimulateViewModel> C341TaskSimulateViewModel { get; set; } = default!;
    }
}
