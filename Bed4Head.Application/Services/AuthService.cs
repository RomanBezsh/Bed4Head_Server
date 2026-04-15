using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity; 

namespace Bed4Head.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _db;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IConfiguration config, IUnitOfWork db)
        {
            _config = config;
            _db = db;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserDTO?> RegisterAsync(RegisterRequestDTO dto)
        {
            var allUsers = await _db.Users.GetAllAsync();
            if (allUsers.Any(u => u.Email.ToLower() == dto.Email.ToLower()))
                return null;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                DisplayName = dto.Name ?? $"{dto.FirstName} {dto.LastName}".Trim(),
                Country = dto.Country,
                City = dto.City,
                TravelPurpose = dto.TravelPurpose,
                BirthDate = dto.Birth,
                CreatedAt = DateTime.UtcNow,
                IsEmailConfirmed = false,
                PasswordSalt = Guid.NewGuid().ToString()
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _db.Users.AddAsync(user);
            await _db.CompleteAsync();

            return MapToDto(user);
        }

        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            var users = await _db.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user == null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public string GenerateToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.DisplayName ?? "User") 
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static UserDTO MapToDto(User u) => new UserDTO
        {
            Id = u.Id,
            Email = u.Email,
            DisplayName = u.DisplayName,
            Country = u.Country,
            City = u.City,
            BirthDate = u.BirthDate,
            CreatedAt = u.CreatedAt,
            IsEmailConfirmed = u.IsEmailConfirmed ?? false
        };
    }
}

