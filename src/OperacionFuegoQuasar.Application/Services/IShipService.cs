using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Application.Requests;
using OperacionFuegoQuasar.Domain.Entities;
namespace OperacionFuegoQuasar.Aplication.Services;
public interface IShipService
{
    ShipLocation GetLocation(float[] distances);
    ShipMessage GetMessage(string[] messages);
    Task<TopSecretDecoded> DecodeTopSecretInfoAsync(TopSecret satellites);
}
