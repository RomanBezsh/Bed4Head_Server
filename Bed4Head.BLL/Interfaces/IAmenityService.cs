using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
{
    public interface IAmenityService
    {
        Task<IEnumerable<AmenityDTO>> GetAllAsync();
        Task<AmenityDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(AmenityDTO dto);
        Task UpdateAsync(AmenityDTO dto);
        Task DeleteAsync(Guid id);
    }
}