using Microsoft.AspNetCore.Mvc;
using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Repositories;
using OperacionFuegoQuasar.Domain.Services;

namespace OperacionFuegoQuasar.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class TopSecretController : ControllerBase
{
    private readonly ISatelliteDataRepository _satelliteDataRepository;
    private readonly IShipService _shipService;

    public TopSecretController(ISatelliteDataRepository satelliteDataRepository, IShipService shipService)
    {
        _satelliteDataRepository = satelliteDataRepository;
        _shipService = shipService;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<TopSecretDecoded> PostAsync(IEnumerable<SatelliteDataReceveid> satellites)
        => await _shipService.DecodeTopSecretInfoAsync(satellites);

    [HttpPost("topsecret_split/{satelliteName}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task Split(SatelliteDataReceveid satellieData) 
        => await _satelliteDataRepository.AddAsync(new SatelliteData(satellieData.Name, satellieData.Distance, string.Join(",", satellieData.Message)));

}
