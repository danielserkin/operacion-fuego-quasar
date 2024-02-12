using Microsoft.EntityFrameworkCore;
using OperacionFuegoQuasar.Domain.Repositories;

namespace OperacionFuegoQuasar.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<SatelliteData> SatelliteData { get; set; }
}