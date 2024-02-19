using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using OperacionFuegoQasar.Api.Models;
using OperacionFuegoQuasar.Application.Requests;
using OperacionFuegoQuasar.Domain.Entities;

namespace IntegrationTests;


public class TopSecretControllerIntegrationTests
{
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }

    [Test]
    public async Task PostAsync_WithValidData_ReturnsDecodedInfo()
    {
        // Arrange
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://pasalocalhost:5001/") 
        });

        var request = new TopSecret
        {
            Satellites = new[]
            {
                new Satellite { Name = "kenobi", Distance = 100, Message = new[] { "este", "", "es", "", "mensaje" } },
                new Satellite { Name = "skywalker", Distance = 115.5f, Message = new[] { "", "es", "", "mensaje", "" } },
                new Satellite { Name = "sato", Distance = 142.7f, Message = new[] { "este", "", "un", "", "" } }
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("topsecret", content); 

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var decodedInfo = await response.Content.ReadFromJsonAsync<TopSecretDecoded>();
        Assert.IsNotNull(decodedInfo);
       
    }


    [Test]
    public async Task SplitAsync_WithValidData_ReturnsDecodedInfo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new TopSecretSplit
        {
            Distance = 100,
            Message = new[] { "este", "", "es", "", "mensaje" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/satelite/topsecret_split/kenobi", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var decodedInfo = await response.Content.ReadFromJsonAsync<TopSecretDecoded>();
        Assert.IsNotNull(decodedInfo);
        // Add more assertions as needed
    }

    [Test]
    public async Task SplitAsync_WithInvalidData_ReturnsError()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new TopSecretSplit
        {
            Distance = -100, // Datos inválidos
            Message = new[] { "este", "", "es", "", "mensaje" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/satelite/topsecret_split/kenobi", content);

        // Assert
        Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound);
        // Add more assertions as needed
    }

    [Test]
    public async Task GetSatelliteInfoAsync_ReturnsSatelliteInfo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var satelliteName = "kenobi";

        // Act
        var response = await client.GetAsync($"/satelite/{satelliteName}");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var satelliteInfo = await response.Content.ReadFromJsonAsync<SatelliteData>();
        Assert.IsNotNull(satelliteInfo);
        Assert.AreEqual(satelliteName, satelliteInfo.Name);
        // Add more assertions as needed
    }

}

