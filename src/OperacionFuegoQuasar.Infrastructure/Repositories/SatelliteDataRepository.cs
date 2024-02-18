using Microsoft.EntityFrameworkCore;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Repositories;
using OperacionFuegoQuasar.Infrastructure.Data;

namespace OperacionFuegoQuasar.Infrastructure.Repositories;

public class SatelliteDataRepository : ISatelliteDataRepository
{
    private readonly ApplicationDbContext _context;

    public SatelliteDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(SatelliteData satelliteData)
    {
        _context.SatelliteData.Add(satelliteData);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllDataFromTablAsync()
    {
        // Get the DbSet representing the table
        var tableEntities = _context.SatelliteData;

        // Remove all entities from the DbSet
        tableEntities.RemoveRange(tableEntities);

        // Save the changes to the database
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SatelliteData>> GetAllSatelliteDataAsync()
        => await _context.SatelliteData.ToListAsync();
}
