using InventorySystem_API.Model;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_API.DataConnection
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(DbContextOptions<DbContextClass> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Login>().HasNoKey(); // Configure Login as a keyless entity
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; } // Add DbSet for Login if needed

  }
}
