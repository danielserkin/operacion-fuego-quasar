using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<object> Post(SatelliteData[] satellites)
    {
        if (satellites == null || satellites.Length != 3)
        {
            return BadRequest("Exactly 3 satellites are required to determine the location and the message.");
        }

        var (distances, messages) = _satelliteDataRepository.GetAllSatelliteData();

        // Procesar la información para determinar la ubicación y el mensaje
        var location = _shipService.GetLocation(distances);
        var message = _shipService.GetMessage(messages);

        return Ok(new { position = location, message = message });
    }

    [HttpPost("topsecret_split/{satelliteName}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult PostSplit(string satelliteName, SatelliteData data)
    {
        // Almacenar la información del satélite específico
        _satelliteDataRepository.Add(data);

        return Ok();
    }

    [HttpGet("topsecret_split")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public ActionResult<object> GetSplit()
    {
        var (distances, messages) = _satelliteDataRepository.GetAllSatelliteData();

        // Obtener la ubicación y el mensaje si es posible determinarlo
        var location = _shipService.GetLocation(distances);
        var message = _shipService.GetMessage(messages);

        if (location == null || message == null)
        {
            return NotFound("No se puede determinar la posición o el mensaje.");
        }

        return Ok(new { position = location, message });
    }
}
