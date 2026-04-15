using Bed4Head.Application.DTOs;
using Bed4Head.Application.Services;
using Bed4Head.Infrastructure.Data;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class BookingServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task CreateBooking_ShouldAdd()
    {
        using var context = GetDbContext();
        var uow = new UnitOfWork(context);
        var service = new BookingService(uow);

        // Create related user and room
        var user = new Bed4Head.Domain.Entities.User { Id = Guid.NewGuid(), Email = "u@example.com", PasswordHash = "x", PasswordSalt = "" };
        await uow.Users.AddAsync(user);
        var hotel = new Bed4Head.Domain.Entities.Hotel { Id = Guid.NewGuid(), Name = "H", Description = "d", Stars = 3, Address = "a", City = "c", Latitude = 0, Longitude = 0, HotelType = "t" };
        await uow.Hotels.AddAsync(hotel);
        var room = new Bed4Head.Domain.Entities.Room { Id = Guid.NewGuid(), HotelId = hotel.Id, Price = 10 };
        await uow.Rooms.AddAsync(room);
        await uow.CompleteAsync();

        var dto = new BookingDTO { UserId = user.Id, RoomId = room.Id, CheckIn = DateTime.UtcNow.Date, CheckOut = DateTime.UtcNow.Date.AddDays(1), TotalPrice = 10 };
        await service.CreateAsync(dto);

        var bookings = (await uow.Bookings.GetAllAsync()).ToList();
        Assert.Single(bookings);
        Assert.Equal(user.Id, bookings[0].UserId);
    }
}


