using Microsoft.AspNetCore.Mvc;
using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Repositories;
using OperacionFuegoQuasar.Domain.Services;

namespace OperacionFuegoQasar.Api.Controllers;

[ApiController]
[Route("[controller]")]
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

        var satelliteDataModel =  satelliteDataFilter.Select(x => new SatelliteDataReceveid() { Name = x.Name, Distance = x.Distance, Message = x.Message.Split(",") });

        return await _shipService.DecodeTopSecretInfoAsync(satelliteDataModel);
}
}
