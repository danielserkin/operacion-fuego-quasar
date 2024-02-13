
using OperacionFuegoQuasar.Domain.Entities;

namespace OperacionFuegoQuasar.Domain.Repositories
{
    public interface ISatelliteDataRepository
    {
        Task AddAsync(SatelliteData satelliteData);
        Task<IEnumerable<SatelliteData>> GetAllSatelliteDataAsync();
    }
}
