using Microsoft.AspNetCore.Mvc;
using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Aplication.Services;
using OperacionFuegoQuasar.Application.Requests;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Repositories;

namespace OperacionFuegoQasar.Api.Controllers;

[ApiController]
[Route("satelite")]
[Produces("application/json")]
public class SatelliteController : ControllerBase
{
    private readonly ISatelliteDataRepository _satelliteDataRepository;
    private readonly IShipService _shipService;

    public SatelliteController(ISatelliteDataRepository satelliteDataRepository, IShipService shipService)
    {
        _satelliteDataRepository = satelliteDataRepository ?? throw new ArgumentNullException(nameof(satelliteDataRepository));
        _shipService = shipService ?? throw new ArgumentNullException(nameof(shipService));
    }

    [HttpGet("topsecret_split")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<TopSecretDecoded> GetSplitAsync()
    {
        var allSatelliteData = await _satelliteDataRepository.GetAllSatelliteDataAsync();
        var satelliteDataFilter = allSatelliteData
            .OrderByDescending(x => x.Id)
            .Take(3);

        var satelliteData = satelliteDataFilter.Select(x => new Satellite() { Name = x.Name, Distance = x.Distance, Message = x.Message.Split(",") });

        return await _shipService.DecodeTopSecretInfoAsync(new TopSecret() { Satellites = satelliteData });

    }
}
