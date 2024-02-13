using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Repositories;
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
    private readonly ISatelliteDataRepository _satelliteDataRepository;

    public ShipService(ISatelliteDataRepository satelliteDataRepository) 
        => _satelliteDataRepository = satelliteDataRepository ?? throw new ArgumentNullException(nameof(satelliteDataRepository));

    public async Task<TopSecretDecoded> DecodeTopSecretInfoAsync(IEnumerable<SatelliteDataReceveid> satellites)
    {
        if (satellites == null || satellites.Count() != 3)
            throw new ArgumentException("Exactly 3 satellites are required to determine the location and the message.");

        foreach (var sateliteData in satellites.ToList())
        {
            var sateliteDataNew = new SatelliteData(sateliteData.Name, sateliteData.Distance, string.Join(",", sateliteData.Message));
            await _satelliteDataRepository.AddAsync(sateliteDataNew);
        }

        var allSatelliteData = await _satelliteDataRepository.GetAllSatelliteDataAsync();

        var location = GetLocation(allSatelliteData.Select(x=>x.Distance).ToArray());
        var message = GetMessage(allSatelliteData.Select(x => x.Message).ToArray());

        return new TopSecretDecoded { Location = location, Message = message };
    }

    public ShipLocation GetLocation(float[] distances)
    {
        if (distances == null || distances.Length != 3)
        {
            throw new ArgumentException("Se requieren exactamente 3 distancias para la triangulación.");
        }

        float d = 2 * (KenobiX * (SkywalkerY - SatoY) + SkywalkerX * (SatoY - KenobiY) + SatoX * (KenobiY - SkywalkerY));
        float px = (distances[0] * (SkywalkerY - SatoY) + distances[1] * (SatoY - KenobiY) + distances[2] * (KenobiY - SkywalkerY)) / d;
        float py = (distances[0] * (SatoX - KenobiX) + distances[1] * (KenobiX - SkywalkerX) + distances[2] * (SkywalkerX - SatoX)) / d;

        return new ShipLocation { X = px, Y = py };
    }

    public ShipMessage GetMessage(string[] messages)
    {
        if (messages.Length < 3)
            throw new ArgumentException("At least three messages are required to construct the final message.");


        // Join all messages into a single string
        string combinedMessage = string.Join(" ", messages);

        // Split the combined string into words
        string[] words = combinedMessage.Split(" ");

        // Filter out empty or null words
        string[] filteredWords = words.Where(word => !string.IsNullOrEmpty(word)).ToArray();

        // Ensure that there are enough words to construct the final message
        if (filteredWords.Length < 3)
        {
            throw new InvalidOperationException("At least three non-empty words are required to construct the final message.");
        }

        // Join the filtered words into the final message
        string finalMessage = string.Join(" ", filteredWords);


        return new ShipMessage() { Message = finalMessage };
    }
}