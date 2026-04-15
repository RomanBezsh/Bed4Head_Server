using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IHotelChainService
    {
        Task<IEnumerable<HotelChainDTO>> GetAllAsync();
        Task<HotelChainDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(HotelChainDTO dto);
        Task UpdateAsync(HotelChainDTO dto);
        Task DeleteAsync(Guid id);
    }
}

