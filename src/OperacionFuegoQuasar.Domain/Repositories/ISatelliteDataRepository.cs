
namespace OperacionFuegoQuasar.Domain.Repositories
{
    public interface ISatelliteDataRepository
    {
        void Add(SatelliteData satelliteData);
        (float[], string[][]) GetAllSatelliteData();
    }
}
