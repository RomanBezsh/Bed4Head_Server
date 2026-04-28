using Bed4Head.Application.DTOs;
using Bed4Head.Application.Services;
using Bed4Head.Infrastructure.Data;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class HotelServiceCoordinatesTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _options;

    public HotelServiceCoordinatesTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new AppDbContext(_options);
        context.Database.EnsureCreated();
    }

    private AppDbContext CreateContext() => new AppDbContext(_options);

    [Fact]
    public async Task CreateAdminAsync_ShouldSaveCoordinates_WhenUsingDotDecimals()
    {
        using var context = CreateContext();
        var service = new HotelService(new UnitOfWork(context), context);

        await service.CreateAdminAsync(new CreateAdminHotelDTO
        {
            Name = "Dot Hotel",
            Address = "Main street",
            City = "Sofia",
            Country = "Bulgaria",
            Coordinates = "42.6977, 23.3219"
        });

        var hotel = await context.Hotels.SingleAsync();
        Assert.Equal(42.6977, hotel.Latitude, 4);
        Assert.Equal(23.3219, hotel.Longitude, 4);
    }

    [Fact]
    public async Task CreateAdminAsync_ShouldSaveCoordinates_WhenUsingCommaDecimals()
    {
        using var context = CreateContext();
        var service = new HotelService(new UnitOfWork(context), context);

        await service.CreateAdminAsync(new CreateAdminHotelDTO
        {
            Name = "Comma Hotel",
            Address = "Main street",
            City = "Sofia",
            Country = "Bulgaria",
            Coordinates = "42,6977; 23,3219"
        });

        var hotel = await context.Hotels.SingleAsync();
        Assert.Equal(42.6977, hotel.Latitude, 4);
        Assert.Equal(23.3219, hotel.Longitude, 4);
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
