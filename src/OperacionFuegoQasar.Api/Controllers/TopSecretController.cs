// OperacionFuegoQuasar.Api/Controllers/TopSecretController.cs
using Microsoft.AspNetCore.Mvc;
using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Domain.Services;

namespace OperacionFuegoQuasar.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TopSecretController : ControllerBase
    {
        private readonly IShipService _shipService;

        public TopSecretController(IShipService shipService)
        {
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

            var distances = new float[] { satellites[0].Distance, satellites[1].Distance, satellites[2].Distance };
            var messages = new string[][] { satellites[0].Message, satellites[1].Message, satellites[2].Message };

            var location = _shipService.GetLocation(distances);
            var message = _shipService.GetMessage(messages);

            return Ok(new { position = location, message = message });
        }
    }
}
