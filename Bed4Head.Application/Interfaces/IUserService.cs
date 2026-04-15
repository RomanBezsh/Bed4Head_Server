using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetByIdAsync(Guid id);
        Task<UserDTO?> GetByEmailAsync(string email);
        Task UpdateAsync(UserDTO dto);
        Task DeleteAsync(Guid id);
        Task UpdateNewsPreferencesAsync(Guid userId, bool seasonal, bool favorite, bool world, bool affordable);
    }
}

