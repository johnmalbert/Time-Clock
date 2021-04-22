using Microsoft.EntityFrameworkCore;
namespace Time_Clock.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<TimeStamp> TimeStamps { get; set; }
    }
}   
