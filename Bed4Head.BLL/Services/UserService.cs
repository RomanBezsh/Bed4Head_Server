using BCrypt.Net;
using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Bed4Head.DAL.Entities;
using Bed4Head.DAL.Repositories;

public class UserService : IUserService
{
    private readonly IUnitOfWork _db;

    public UserService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        var users = await _db.Users.GetAllAsync();
        return users.Select(u => new UserDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        });
    }

    public async Task<UserDTO?> GetUserByEmailAsync(string email)
    {
        var users = await _db.Users.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null) return null;
        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task RegisterAsync(UserDTO userDto, string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();

        string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            Email = userDto.Email,
            PasswordHash = hash,
            PasswordSalt = salt 
        };

        await _db.Users.AddAsync(user);
    }

    public async Task<bool> VerifyPasswordAsync(string email, string password)
    {
        var users = await _db.Users.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == email);

        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}