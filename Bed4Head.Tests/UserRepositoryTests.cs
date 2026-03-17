using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Bed4Head.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit; // Не забудь добавить, если не было

public class UserRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        // Используем SQLite In-Memory для тестов
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    private static string ToBase64(byte[] data) => Convert.ToBase64String(data);

    [Fact]
    public async Task AddAsync_ShouldCreateUser()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            DisplayName = "Roma", // Исправлено: Name -> DisplayName
            Email = "roma@example.com",
            PasswordHash = ToBase64(new byte[] { 1, 2, 3 }),
            PasswordSalt = ToBase64(new byte[] { 4, 5, 6 })
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync(); // Сохраняем вручную в тесте

        var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "roma@example.com");
        Assert.NotNull(dbUser);
        Assert.Equal("Roma", dbUser.DisplayName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            DisplayName = "Anna", // Исправлено
            Email = "anna@example.com",
            PasswordHash = ToBase64(new byte[] { 7, 8, 9 }),
            PasswordSalt = ToBase64(new byte[] { 10, 11, 12 })
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync(); // Нужно сохранить, чтобы FindAsync его нашел

        var fetched = await repository.GetByIdAsync(user.Id);
        Assert.NotNull(fetched);
        Assert.Equal("Anna", fetched!.DisplayName);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);

        var user1 = new User
        {
            Id = Guid.NewGuid(),
            DisplayName = "User1",
            Email = "u1@example.com",
            PasswordHash = ToBase64(new byte[] { 1 }),
            PasswordSalt = ToBase64(new byte[] { 1 })
        };
        var user2 = new User
        {
            Id = Guid.NewGuid(),
            DisplayName = "User2",
            Email = "u2@example.com",
            PasswordHash = ToBase64(new byte[] { 2 }),
            PasswordSalt = ToBase64(new byte[] { 2 })
        };

        await repository.AddAsync(user1);
        await repository.AddAsync(user2);
        await context.SaveChangesAsync();

        var users = (await repository.GetAllAsync()).ToList();
        Assert.True(users.Count >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyUser()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            DisplayName = "Before",
            Email = "update@example.com",
            PasswordHash = ToBase64(new byte[] { 3 }),
            PasswordSalt = ToBase64(new byte[] { 3 })
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        // Обновляем
        user.DisplayName = "After";
        await repository.UpdateAsync(user);
        await context.SaveChangesAsync(); // Сохраняем изменения

        var updated = await context.Users.FindAsync(user.Id);
        Assert.NotNull(updated);
        Assert.Equal("After", updated!.DisplayName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            DisplayName = "ToDelete",
            Email = "delete@example.com",
            PasswordHash = ToBase64(new byte[] { 5 }),
            PasswordSalt = ToBase64(new byte[] { 5 })
        };

        await repository.AddAsync(user);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(user.Id);
        await context.SaveChangesAsync(); // Применяем удаление

        var deleted = await context.Users.FindAsync(user.Id);
        Assert.Null(deleted);
    }
}