using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
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