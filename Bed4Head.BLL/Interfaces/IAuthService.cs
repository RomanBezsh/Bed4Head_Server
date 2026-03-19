using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(UserDTO user);
        Task<UserDTO?> RegisterAsync(RegisterRequestDTO dto);
        Task<bool> VerifyPasswordAsync(string email, string password);
    }
}
