using Microsoft.EntityFrameworkCore;
using SecureSoftware.Entities;

namespace SecureSoftware.DataAccess
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<MapPoint> MapPoints { get; set; }  

    }
}
