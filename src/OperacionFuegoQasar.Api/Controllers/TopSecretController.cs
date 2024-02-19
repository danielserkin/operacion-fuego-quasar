using Microsoft.AspNetCore.Mvc;
using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Aplication.Services;
using OperacionFuegoQuasar.Application.Requests;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Repositories;

namespace OperacionFuegoQuasar.Api.Controllers;

[ApiController]
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


    [HttpPost("topsecret")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TopSecretDecoded>> PostAsync(TopSecret topSecret)
    {
        if (topSecret.Satellites != null)
        {
            var decodedInfo = await _shipService.DecodeTopSecretInfoAsync(topSecret);
            return Ok(decodedInfo);
        }

        return BadRequest();
    }

    [HttpPost("topsecret_split/{satelliteName}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task Split(string satelliteName, TopSecretSplit topSecretSplit) 
        => await _satelliteDataRepository.AddAsync(new SatelliteData(satelliteName, topSecretSplit.Distance, string.Join(",", topSecretSplit.Message)));

}
