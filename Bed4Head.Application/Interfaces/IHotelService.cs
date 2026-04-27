using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelSummaryDTO>> GetAllSummariesAsync();
        Task<HotelDetailsDTO?> GetByIdAsync(Guid id);
        Task<HotelFullDTO?> GetFullByIdAsync(Guid id);
        Task<Guid> CreateAsync(HotelDetailsDTO dto);
        Task<Guid> CreateAdminAsync(CreateAdminHotelDTO dto);
        Task UpdateAsync(HotelDetailsDTO dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<HotelSummaryDTO>> GetByChainIdAsync(Guid chainId);
    }
}

