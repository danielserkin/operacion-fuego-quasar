using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Services;

namespace OperacionFuegoQuasar.Application.Services;
public class ShipService : IShipService
{
    private const float KenobiX = -500;
    private const float KenobiY = -200;
    private const float SkywalkerX = 100;
    private const float SkywalkerY = -100;
    private const float SatoX = 500;
    private const float SatoY = 100;

    public ShipLocation GetLocation(float[] distances)
    {
        if (distances == null || distances.Length != 3)
        {
            throw new ArgumentException("Se requieren exactamente 3 distancias para la triangulación.");
        }

        float x, y;

        // Triangulación de la posición
        var d1 = distances[0];
        var d2 = distances[1];
        var d3 = distances[2];

        // Fórmula para calcular la ubicación del emisor del mensaje
        x = (float)((Math.Pow(d1, 2) - Math.Pow(d2, 2) + Math.Pow(KenobiX, 2)) / (2 * KenobiX));
        y = (float)(((Math.Pow(d1, 2) - Math.Pow(d3, 2) + Math.Pow(KenobiX, 2) + Math.Pow(KenobiY, 2)) / (2 * KenobiY)) - ((KenobiX / KenobiY) * x));

        return new ShipLocation { X = x, Y = y };
    }

    public ShipMessage GetMessage(string[][] messages)
    {
        if (messages == null || messages.Length != 3)
        {
            throw new ArgumentException("Se requieren exactamente 3 mensajes para reconstruir el mensaje original.");
        }

        // Reconstruir el mensaje original
        var reconstructedMessage = Enumerable.Range(0, messages.Max(m => m.Length))
            .Select(i => messages.Select(m => m.ElementAtOrDefault(i)).FirstOrDefault())
            .SelectMany(word => word ?? string.Empty)
            .ToArray();

        return new ShipMessage { Message = string.Join(" ", reconstructedMessage) };
    }
}