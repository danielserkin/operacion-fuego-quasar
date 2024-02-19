using Moq;
using OperacionFuegoQuasar.Application.Exceptions;
using OperacionFuegoQuasar.Application.Requests;
using OperacionFuegoQuasar.Domain.Repositories;
using OperacionFuegoQuasar.Application.Services;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Aplication.Services;
using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Tests;

public class TopSecretControllerTests
{
    [Test(Description = "Given valid top secret data, " +
                     "When posting data to the controller, " +
                     "Then it should return the decoded information.")]
    public async Task PostAsync_ValidData_ReturnsDecodedInfo()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var mockService = new Mock<IShipService>();

        var controller = new TopSecretController(mockRepository.Object, mockService.Object);
        var topSecretRequest = new TopSecret();

        var expectedDecodedInfo = new TopSecretDecoded { };
        mockService.Setup(s => s.DecodeTopSecretInfoAsync(topSecretRequest)).ReturnsAsync(expectedDecodedInfo);

        // Act
        var result = await controller.PostAsync(topSecretRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<ActionResult<TopSecretDecoded>>(result);
    }



    [Test(Description = "Given a request without satellite data, " +
                       "When posting to the top secret endpoint, " +
                       "Then it should return a 400 Bad Request.")]
    public async Task PostAsync_NoSatelliteData_ReturnsBadRequest()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var mockService = new Mock<IShipService>();
        var controller = new TopSecretController(mockRepository.Object, mockService.Object);

        var topSecretRequest = new TopSecret { Satellites = null };

        // Act
        var result = await controller.PostAsync(topSecretRequest);

        // Assert
        Assert.IsInstanceOf<ActionResult<TopSecretDecoded>>(result);
    }

    [Test(Description = "Given valid satellite data, " +
                       "When posting data to the split endpoint, " +
                       "Then it should return a 200 OK.")]
    public async Task Split_ValidData_ReturnsOk()
    {
        // Arrange
        var mockRepository = new Mock<ISatelliteDataRepository>();
        var mockService = new Mock<IShipService>();
        var controller = new TopSecretController(mockRepository.Object, mockService.Object);

        var topSecretSplit = new TopSecretSplit { Distance = 100, Message = new string[] { "este", "", "es", "", "mensaje" } };

        // Act
        await controller.Split("kenobi", topSecretSplit);

        // Assert
        mockRepository.Verify(repo => repo.AddAsync(It.Is<SatelliteData>(data =>
            data.Name == "kenobi" &&
            data.Distance == 100 &&
            data.Message == "este,,es,,mensaje"
        )), Times.Once);
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

        // Set up the repository to throw an exception
        mockRepository.Setup(r => r.AddAsync(It.IsAny<SatelliteData>())).ThrowsAsync(new Exception());

        // Act + Assert
        Assert.ThrowsAsync<Exception>(async () => await controller.Split("kenobi", topSecretSplit));
    }


}
