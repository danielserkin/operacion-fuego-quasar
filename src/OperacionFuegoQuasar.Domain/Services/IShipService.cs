using OperacionFuegoQuasar.Domain.Repositories;
namespace OperacionFuegoQuasar.Domain.Services;
public interface IShipService
{
    ShipLocation GetLocation(float[] distances);
    ShipMessage GetMessage(string[][] messages);
}
