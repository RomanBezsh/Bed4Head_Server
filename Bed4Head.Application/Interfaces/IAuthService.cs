using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(UserDTO user);
        Task<UserDTO?> RegisterAsync(RegisterRequestDTO dto);
        Task<bool> VerifyPasswordAsync(string email, string password);
        Task<bool> VerifyConfirmationCodeAsync(string email, string code);
        Task<bool> ConfirmEmailAsync(string email);
        Task<bool> UpdateProfileAsync(UpdateProfileRequestDTO dto);
    }
}


