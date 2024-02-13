using Microsoft.EntityFrameworkCore;
using OperacionFuegoQuasar.Domain.Entities;

namespace OperacionFuegoQuasar.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SatelliteData> SatelliteData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=operacion-fuego-qasar.db");
            }
        }
    }
}
