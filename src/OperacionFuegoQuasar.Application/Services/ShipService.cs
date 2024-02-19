using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Aplication.Services;
using OperacionFuegoQuasar.Application.Exceptions;
using OperacionFuegoQuasar.Application.Requests;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Repositories;

namespace OperacionFuegoQuasar.Application.Services;
public class ShipService : IShipService
{
    private const float KenobiX = -500f;
    private const float KenobiY = -200f;
    private const float SkywalkerX = 100f;
    private const float SkywalkerY = -100f;
    private const float SatoX = 500f;
    private const float SatoY = 100f;
    private readonly ISatelliteDataRepository _satelliteDataRepository;

    public ShipService(ISatelliteDataRepository satelliteDataRepository) 
        => _satelliteDataRepository = satelliteDataRepository ?? throw new ArgumentNullException(nameof(satelliteDataRepository));

    public async Task<TopSecretDecoded> DecodeTopSecretInfoAsync(TopSecret topSecret)
    {
        ValidateRequest(topSecret);

        await _satelliteDataRepository.DeleteAllDataFromTablAsync();
        foreach (var sateliteData in topSecret.Satellites.ToList())
        {
            var sateliteDataNew = new SatelliteData(sateliteData.Name, sateliteData.Distance, string.Join(",", sateliteData.Message));
            await _satelliteDataRepository.AddAsync(sateliteDataNew);
        }

        var allSatelliteData = await _satelliteDataRepository.GetAllSatelliteDataAsync();

        var location = GetLocation(allSatelliteData.Select(x=>x.Distance).ToArray());
        var message = GetMessage(allSatelliteData.Select(x => x.Message).ToArray());

        return new TopSecretDecoded { Location = location, Message = message };
    }

    private void ValidateRequest(TopSecret topSecret)
    {
        if (topSecret.Satellites == null || topSecret.Satellites.Count() != 3)
            throw new InvalidNumbersOfSatellitesException();

        if (topSecret.Satellites.Any(x => x.Message == null || string.Join("",x.Message) == string.Empty))
            throw new IncorrectMessageException();

        if (topSecret.Satellites.Any(x => x.Distance == 0))
            throw new InvalidDistanceException();
    }

    public ShipLocation GetLocation(float[] distances)
    {
        if (distances == null || distances.Length != 3)
            throw new InvalidNumbersOfDistancesException();

        float d12 = distances[0] * distances[0];
        float d22 = distances[1] * distances[1];
        float d32 = distances[2] * distances[2];

        float X = (d12 - d22 + KenobiX * KenobiX) / (2 * KenobiX);

 
        float Y = ((d12 - d32 + SkywalkerX * SkywalkerX + SatoX * SatoY) / (2 * SkywalkerY)) -
                  (SkywalkerX / SkywalkerY) * X;


        return new ShipLocation()
        {
            X = X,
            Y = Y
        };
    }

    public ShipMessage GetMessage(string[] messages)
    {
        if (messages.Length < 3)
            throw new InvalidNumbersOfMessagesException();

        var wordPositions = messages.Select((m, index) =>
        {
            var words = m.Split(',', StringSplitOptions.None)
                         .Select((word, position) => (word.Trim(), position))
                         .Where(wp => !string.IsNullOrEmpty(wp.Item1))
                         .ToList();
            return words;
        });

        var uniqueWords = wordPositions
            .SelectMany(list => list)
            .OrderBy(wp => wp.position)
            .Select(wp => wp.Item1)
            .Distinct();

        return new ShipMessage() { Message = string.Join(" ", uniqueWords) };
    }
}