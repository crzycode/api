using Microsoft.EntityFrameworkCore;

namespace api.Models.DataContext
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {

        }
        public DbSet<User>? Users { get; set; }
    }
}
