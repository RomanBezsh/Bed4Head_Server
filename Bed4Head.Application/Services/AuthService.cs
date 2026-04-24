using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bed4Head.Application.Services
{
    public class AuthService : IAuthService
    {
        private static readonly ConcurrentDictionary<string, VerificationCodeEntry> _codes = new(StringComparer.OrdinalIgnoreCase);
        private static readonly TimeSpan VerificationCodeTtl = TimeSpan.FromMinutes(10);

        private readonly IConfiguration _config;
        private readonly IUnitOfWork _db;
        private readonly IEmailService _emailService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IConfiguration config, IUnitOfWork db, IEmailService emailService)
        {
            _config = config;
            _db = db;
            _emailService = emailService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserDTO?> RegisterAsync(RegisterRequestDTO dto)
        {
            var allUsers = await _db.Users.GetAllAsync();
            if (allUsers.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
                return null;

            var user = new User
            {
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow,
                IsEmailConfirmed = false,
                PasswordSalt = Guid.NewGuid().ToString()
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _db.Users.AddAsync(user);
            await _db.CompleteAsync();

            var verificationCode = GenerateVerificationCode();
            SaveVerificationCode(user.Email, verificationCode);
            await _emailService.SendVerificationCodeAsync(user.Email, verificationCode);

            return MapToDto(user);
        }

        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            var users = await _db.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public Task<bool> VerifyConfirmationCodeAsync(string email, string code)
        {
            return Task.FromResult(VerifyConfirmationCode(email, code));
        }

        public async Task<bool> ConfirmEmailAsync(string email)
        {
            var users = await _db.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return false;
            }

            user.IsEmailConfirmed = true;
            await _db.Users.UpdateAsync(user);
            await _db.CompleteAsync();

            return true;
        }

        public async Task<bool> UpdateProfileAsync(UpdateProfileRequestDTO dto)
        {
            var users = await _db.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return false;
            }

            user.DisplayName = dto.Name;
            user.Country = dto.Country;
            user.City = dto.City;
            user.TravelPurpose = dto.TravelPurpose;

            await _db.Users.UpdateAsync(user);
            await _db.CompleteAsync();

            return true;
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

        private static string GenerateVerificationCode()
        {
            return RandomNumberGenerator.GetInt32(100000, 1_000_000).ToString();
        }

        private static void SaveVerificationCode(string email, string code)
        {
            CleanupExpiredVerificationCodes();

            var key = email.Trim();
            var entry = new VerificationCodeEntry(code, DateTimeOffset.UtcNow.Add(VerificationCodeTtl));

            _codes.AddOrUpdate(key, entry, (_, _) => entry);
        }

        private static bool VerifyConfirmationCode(string email, string code)
        {
            CleanupExpiredVerificationCodes();

            var key = email.Trim();
            if (_codes.TryGetValue(key, out var entry) &&
                entry.Expiration > DateTimeOffset.UtcNow &&
                string.Equals(entry.Code, code, StringComparison.Ordinal))
            {
                _codes.TryRemove(key, out _);
                return true;
            }

            return false;
        }

        private static void CleanupExpiredVerificationCodes()
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var pair in _codes)
            {
                if (pair.Value.Expiration <= now)
                {
                    _codes.TryRemove(pair.Key, out _);
                }
            }
        }

        private readonly record struct VerificationCodeEntry(string Code, DateTimeOffset Expiration);

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
