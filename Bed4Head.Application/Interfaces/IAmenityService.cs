using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
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

