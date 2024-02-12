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

    public void Add(SatelliteData satelliteData)
    {
        satelliteData.Timestamp = DateTime.UtcNow;
        _context.SatelliteData.Add(satelliteData);
        _context.SaveChanges();
    }

    public (float[], string[][]) GetAllSatelliteData()
    {
        var satelliteData = _context.SatelliteData.ToList(); 

        var distances = satelliteData.Select(s => s.Distance).ToArray();
        var messages = satelliteData.Select(s => s.Message.Split(",")).ToArray();

        return (distances, messages);
    }

}
