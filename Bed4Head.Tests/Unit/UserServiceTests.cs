using Bed4Head.Application.DTOs;
using Bed4Head.Application.Services;
using Bed4Head.Infrastructure.Data;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _options;

    public UserServiceTests()
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
       public async Task Create_ShouldAddUser()
    {
        // Arrange
        using var context = CreateContext();
        var uow = new UnitOfWork(context);
        var service = new UserService(uow);
        var dto = new UserDTO { Email = "test@example.com", DisplayName = "Tester" };

        await service.CreateAsync(dto, "password123");

        using var checkContext = CreateContext(); 
        var users = await checkContext.Users.ToListAsync();
        Assert.Single(users);
        Assert.Equal("test@example.com", users[0].Email);
    }

    [Fact]
    public async Task Update_ShouldModifyUser()
    {
        using (var context = CreateContext())
        {
            var uow = new UnitOfWork(context);
            var service = new UserService(uow);
            await service.CreateAsync(new UserDTO { Email = "u1@example.com", DisplayName = "Before" }, "pwd");
        }

        using (var context = CreateContext())
        {
            var uow = new UnitOfWork(context);
            var service = new UserService(uow);
            var user = await context.Users.FirstAsync();
            var updateDto = new UserDTO { Id = user.Id, Email = user.Email, DisplayName = "After" };

            await service.UpdateAsync(updateDto);
        }

        using (var checkContext = CreateContext())
        {
            var updated = await checkContext.Users.FirstAsync();
            Assert.Equal("After", updated.DisplayName);
        }
    }

    [Fact]
    public async Task Delete_ShouldRemoveUser()
    {
        Guid userId;
        using (var context = CreateContext())
        {
            var uow = new UnitOfWork(context);
            var service = new UserService(uow);
            var dto = new UserDTO { Email = "del@example.com", DisplayName = "ToDelete" };
            await service.CreateAsync(dto, "pwd");
            userId = (await context.Users.FirstAsync()).Id;
        }

        using (var context = CreateContext())
        {
            var uow = new UnitOfWork(context);
            var service = new UserService(uow);
            await service.DeleteAsync(userId);
        }

        using (var checkContext = CreateContext())
        {
            var deleted = await checkContext.Users.FindAsync(userId);
            Assert.Null(deleted);
        }
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}

