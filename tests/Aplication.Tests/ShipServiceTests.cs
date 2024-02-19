
using Moq;
using OperacionFuegoQuasar.Application.Exceptions;
using OperacionFuegoQuasar.Application.Requests;
using OperacionFuegoQuasar.Domain.Repositories;
using OperacionFuegoQuasar.Application.Services;
using OperacionFuegoQuasar.Domain.Entities;

namespace Aplication.Tests;

[TestFixture]
public class ShipServiceTests
{

    [Test(Description = "Given a valid request with correct satellite data, " +
                   "When decoding top secret info, " +
                   "Then it should decode the information correctly.")]
    public async Task DecodeTopSecretInfoAsync_ValidRequest_DecodesCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var service = new ShipService(mockRepository.Object);

        // Configurar el Mock para devolver tres registros
        var mockData = new List<SatelliteData>
        {
            new SatelliteData("kenobi", 100f, "este,,un,mensaje"),
            new SatelliteData("skywalker", 115.5f, ",es,,secreto"),
            new SatelliteData("sato", 142.7f, "este,,,mensaje,")
        };
        mockRepository.Setup(r => r.GetAllSatelliteDataAsync()).ReturnsAsync(mockData);

        var topSecretRequest = new TopSecret
        {
            Satellites = new List<Satellite>
            {
                new Satellite { Name = "kenobi", Distance = 100f, Message = new string[] { "este", "", "", "mensaje", "" } },
                new Satellite { Name = "skywalker", Distance = 115.5f, Message = new string[] { "", "es", "", "", "secreto" } },
                new Satellite { Name = "sato", Distance = 142.7f, Message = new string[] { "este", "", "un", "", "" } }
            }
        };

        // Act
        var decodedInfo = await service.DecodeTopSecretInfoAsync(topSecretRequest);

        // Assert
        Assert.IsNotNull(decodedInfo);
        Assert.NotNull(decodedInfo.Location.X);
        Assert.NotNull( decodedInfo.Location.Y);
        Assert.AreEqual("este es un mensaje secreto", decodedInfo.Message.Message);
    }


    [Test(Description = "Given a request with an invalid number of satellites, " +
                           "When decoding top secret info, " +
                           "Then it should throw an InvalidNumbersOfSatellitesException.")]
    public async Task DecodeTopSecretInfoAsync_InvalidNumberOfSatellites_ThrowsException()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var service = new ShipService(mockRepository.Object);
        var topSecretRequest = new TopSecret { Satellites = new List<Satellite>() };

        // Act + Assert
        var exception = Assert.ThrowsAsync<InvalidNumbersOfSatellitesException>(
            async () => await service.DecodeTopSecretInfoAsync(topSecretRequest));

        Assert.IsNotNull(exception);
    }

    [Test(Description = "Given a request with missing or incorrect satellite data, " +
                   "When decoding top secret info, " +
                   "Then it should handle the missing or incorrect data gracefully.")]
    public async Task DecodeTopSecretInfoAsync_MissingOrIncorrectSatelliteData_ThrowException()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var service = new ShipService(mockRepository.Object);

        var mockData = new List<SatelliteData>
    {
        new SatelliteData("kenobi", 100f, "este,,un,mensaje"),
        new SatelliteData("skywalker", 115.5f, ",es,,secreto"),
        new SatelliteData("sato", 142.7f, null)
    };
        mockRepository.Setup(r => r.GetAllSatelliteDataAsync()).ReturnsAsync(mockData);

        var topSecretRequest = new TopSecret
        {
            Satellites = new List<Satellite>
            {
                new Satellite { Name = "kenobi", Distance = 100f, Message = new string[] { "este", "", "", "mensaje", "" } },
                new Satellite { Name = "skywalker", Distance = 115.5f, Message = new string[] { "", "es", "", "", "secreto" } },
                new Satellite { Name = "sato", Distance = 142.7f, Message = null } 
            }
        };

        // Act + Assert
        var exception = Assert.ThrowsAsync<IncorrectMessageException>(
            async () => await service.DecodeTopSecretInfoAsync(topSecretRequest));

        Assert.IsNotNull(exception);
    }

    [Test(Description = "Given a request with missing or incorrect distances, " +
                   "When decoding top secret info, " +
                   "Then it should throw an exception indicating the issue.")]
    public async Task DecodeTopSecretInfoAsync_MissingOrIncorrectDistances_ThrowsException()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var service = new ShipService(mockRepository.Object);

        var mockData = new List<SatelliteData>
        {
            new SatelliteData("kenobi", 100f, "este,,un,mensaje"),
            new SatelliteData("skywalker", 115.5f, ",es,,secreto"),
            new SatelliteData("sato", 142.7f, "este,,,mensaje,")
        };
        mockRepository.Setup(r => r.GetAllSatelliteDataAsync()).ReturnsAsync(mockData);

        var topSecretRequest = new TopSecret
        {
            Satellites = new List<Satellite>
            {
                new Satellite { Name = "kenobi", Distance = 100f, Message = new string[] { "este", "", "", "mensaje", "" } },
                new Satellite { Name = "skywalker", Distance = 115.5f, Message = new string[] { "", "es", "", "", "secreto" } },
                new Satellite { Name = "sato", Distance = 0f, Message = new string[] { "este", "", "un", "", "" } }
            }
        };

        // Act + Assert
        var exception = Assert.ThrowsAsync<InvalidDistanceException>(
            async () => await service.DecodeTopSecretInfoAsync(topSecretRequest));

        Assert.IsNotNull(exception);
    }

    [Test(Description = "Given a request with missing or empty messages, " +
                   "When decoding top secret info, " +
                   "Then it should throw an exception indicating the issue.")]
    public async Task DecodeTopSecretInfoAsync_MissingOrEmptyMessages_ThrowsException()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var service = new ShipService(mockRepository.Object);

        // Configurar el Mock para devolver datos de mensajes faltantes o vacíos
        var mockData = new List<SatelliteData>
        {
            new SatelliteData("kenobi", 100f, "este,,un,mensaje"),
            new SatelliteData("skywalker", 115.5f, ",es,,secreto"),
            new SatelliteData("sato", 142.7f, "este,,,mensaje,")
        };
        mockRepository.Setup(r => r.GetAllSatelliteDataAsync()).ReturnsAsync(mockData);

        var topSecretRequest = new TopSecret
        {
            Satellites = new List<Satellite>
            {
                new Satellite { Name = "kenobi", Distance = 100f, Message = new string[] { "este", "", "", "mensaje", "" } },
                new Satellite { Name = "skywalker", Distance = 115.5f, Message = new string[] { "", "es", "", "", "secreto" } },
                new Satellite { Name = "sato", Distance = 142.7f, Message = new string[] { "", "", "", "", "" } } 
            }
        };

        // Act + Assert
        var exception = Assert.ThrowsAsync<IncorrectMessageException>(
            async () => await service.DecodeTopSecretInfoAsync(topSecretRequest));

        Assert.IsNotNull(exception);
    }



}