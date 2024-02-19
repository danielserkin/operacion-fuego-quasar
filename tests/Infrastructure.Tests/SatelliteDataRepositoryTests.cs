using Moq;
using OperacionFuegoQuasar.Domain.Entities;
using OperacionFuegoQuasar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using OperacionFuegoQuasar.Infrastructure.Repositories;
using OperacionFuegoQuasar.Domain.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OperacionFuegoQuasar.Infrastructure.Exceptions;

namespace Infrastructure.Tests;

[TestFixture]
public class SatelliteDataRepositoryTests
{   

    [Test(Description = "Given data in the repository, " +
                   "When trying to delete all satellite data, " +
                   "Then it should remove all data from the database.")]
    public async Task DeleteAllDataFromTablAsync_WithExistingData_RemovesAllDataFromDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Create the in-memory database context
        using (var context = new ApplicationDbContext(options))
        {
            // Populate the database with some sample data
            context.SatelliteData.AddRange(
                new SatelliteData() { Name = "Satellite1", Distance = 100, Message = "Message1" },
                new SatelliteData() { Name = "Satellite2", Distance = 200, Message = "Message2" },
                new SatelliteData() { Name = "Satellite3", Distance = 300, Message = "Message3" }
            );
            context.SaveChanges();
        }

        // Create the repository with the in-memory database context
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new SatelliteDataRepository(context);

            // Act
            await repository.DeleteAllDataFromTablAsync();

            // Assert
            // Ensure that all data has been removed from the database
            Assert.AreEqual(0, context.SatelliteData.Count());
        }
    }

    [Test(Description = "Given valid satellite data, " +
                  "When adding data to the repository, " +
                  "Then it should be added to the database.")]
    public async Task AddAsync_WithValidData_AddsToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            var repository = new SatelliteDataRepository(context);
            var satelliteData = new SatelliteData("Kenobi", 100, "este,es,un,mensaje,secreto");

            // Act
            await repository.AddAsync(satelliteData);

            // Assert
            Assert.AreEqual(1, context.SatelliteData.Count());
        }
    }

    [Test(Description = "Given existing data in the repository, " +
                   "When trying to get all satellite data, " +
                   "Then it should return all data from the database.")]
    public async Task GetAllSatelliteDataAsync_WithExistingData_ReturnsAllDataFromDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Create the in-memory database context
        using (var context = new ApplicationDbContext(options))
        {
            // Populate the database with some sample data
            context.SatelliteData.AddRange(
                new SatelliteData { Name = "Satellite1", Distance = 100, Message = "Message1" },
                new SatelliteData { Name = "Satellite2", Distance = 200, Message = "Message2" },
                new SatelliteData { Name = "Satellite3", Distance = 300, Message = "Message3" }
            );
            context.SaveChanges();
        }

        // Create the repository with the in-memory database context
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new SatelliteDataRepository(context);

            // Act
            var result = await repository.GetAllSatelliteDataAsync();

            // Assert
            // Ensure that all data has been retrieved from the database
            Assert.AreEqual(3, result.Count());
        }
    }

    [Test(Description = "Given no data in the repository, " +
                   "When trying to get all satellite data, " +
                   "Then it should return an empty collection.")]
    public async Task GetAllSatelliteDataAsync_WithNoData_ReturnsEmptyCollection()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase2")
            .Options;

        // Create the repository with the in-memory database context
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new SatelliteDataRepository(context);

            // Act
            var result = await repository.GetAllSatelliteDataAsync();

            // Assert
            Assert.IsEmpty(result);
        }
    }

    [Test(Description = "Given an error occurs while adding data to the repository, " +
                       "When trying to add data, " +
                       "Then it should throw a DbOperationException.")]
    public void AddAsync_ErrorOccurs_ThrowsDbOperationException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Create the repository with the in-memory database context
        using (var context = new ApplicationDbContext(options))
        {
            // Mock a context that throws an exception when saving changes
            var mockSet = new Mock<DbSet<SatelliteData>>();
            mockSet.Setup(m => m.Add(It.IsAny<SatelliteData>())).Throws<Exception>();

            var mockContext = new Mock<ApplicationDbContext>(options);
            mockContext.Setup(c => c.Set<SatelliteData>()).Returns(mockSet.Object);

            var repository = new SatelliteDataRepository(mockContext.Object);
            var satelliteData = new SatelliteData("Kenobi", 100, "este,es,un,mensaje,secreto");

            // Act + Assert
            Assert.ThrowsAsync<DbOperationException>(async () => await repository.AddAsync(satelliteData));
        }
    }


    [Test(Description = "Given an error when retrieving satellite data from the repository, " +
                   "When GetAllSatelliteDataAsync is called, " +
                   "Then it should throw a DbOperationException.")]
    public async Task GetAllSatelliteDataAsync_ErrorOccurs_ThrowsDbOperationException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Create the repository with the in-memory database context
        using (var context = new ApplicationDbContext(options))
        {
            // Mock a context that throws an exception when trying to retrieve satellite data
            var mockSet = new Mock<DbSet<SatelliteData>>();
            var mockContext = new Mock<ApplicationDbContext>(options);
            mockContext.Setup(c => c.Set<SatelliteData>()).Throws<Exception>();

            var repository = new SatelliteDataRepository(mockContext.Object);

            // Act + Assert
            Assert.ThrowsAsync<DbOperationException>(async () => await repository.GetAllSatelliteDataAsync());
        }
    }

    [Test(Description = "Given an error when deleting all satellite data from the repository, " +
                   "When DeleteAllDataFromTablAsync is called, " +
                   "Then it should throw a DbOperationException.")]
    public async Task DeleteAllDataFromTablAsync_ErrorOccurs_ThrowsDbOperationException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Create the repository with the in-memory database context
        using (var context = new ApplicationDbContext(options))
        {
            // Mock a context that throws an exception when trying to delete all satellite data
            var mockSet = new Mock<DbSet<SatelliteData>>();
            var mockContext = new Mock<ApplicationDbContext>(options);
            mockContext.Setup(c => c.Set<SatelliteData>().RemoveRange(It.IsAny<IEnumerable<SatelliteData>>())).Throws<Exception>();

            var repository = new SatelliteDataRepository(mockContext.Object);

            // Act + Assert
            Assert.ThrowsAsync<DbOperationException>(async () => await repository.DeleteAllDataFromTablAsync());
        }
    }






}




