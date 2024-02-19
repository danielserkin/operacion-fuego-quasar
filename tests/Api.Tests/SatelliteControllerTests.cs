using Moq;
using OperacionFuegoQuasar.Api.Controllers;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Domain.Repositories;
using OperacionFuegoQuasar.Aplication.Services;
using OperacionFuegoQasar.Api.Controllers;
using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Application.Requests;
using OperacionFuegoQuasar.Application.Exceptions;

namespace Api.Tests;

[TestFixture]
public class SatelliteControllerTests
{
    [Test(Description = "Given stored satellite data, " +
                        "When getting split top secret info, " +
                        "Then it should return the decoded information.")]
    public async Task GetSplitAsync_ReturnsDecodedInfo()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var mockService = new Mock<IShipService>();

        var expectedDecodedInfo = new TopSecretDecoded { };
        mockService.Setup(s => s.DecodeTopSecretInfoAsync(It.IsAny<TopSecret>())).ReturnsAsync(expectedDecodedInfo);

        var controller = new SatelliteController(mockRepository.Object, mockService.Object);

        // Mock repository to return some satellite data
        var mockSatelliteData = new List<SatelliteData>
        {
            new SatelliteData("Kenobi", 100, "este,es,un,mensaje"),
            new SatelliteData("Skywalker", 200, "un,mensaje"),
            new SatelliteData("Sato", 150, "este,es,otro,mensaje")
        };
        mockRepository.Setup(r => r.GetAllSatelliteDataAsync()).ReturnsAsync(mockSatelliteData);

        // Act
        var result = await controller.GetSplitAsync();

        // Assert
        Assert.AreEqual(expectedDecodedInfo, result);
    }

    [Test(Description = "Given invalid satellite data, " +
                    "When posting data to the split endpoint, " +
                    "Then it should return an internal server error.")]
    public async Task Split_InvalidData_ThrowsInternalServerError()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var mockService = new Mock<IShipService>();
        var controller = new TopSecretController(mockRepository.Object, mockService.Object);

        var topSecretSplit = new TopSecretSplit { Distance = 100, Message = new string[] { "este", "", "es", "", "mensaje" } };

        // Set up the repository to throw a specific exception
        mockRepository.Setup(r => r.AddAsync(It.IsAny<SatelliteData>())).ThrowsAsync(new InvalidNumbersOfSatellitesException());

        // Act + Assert
        Assert.ThrowsAsync<InvalidNumbersOfSatellitesException>(async () => await controller.Split("kenobi", topSecretSplit));
    }


}
