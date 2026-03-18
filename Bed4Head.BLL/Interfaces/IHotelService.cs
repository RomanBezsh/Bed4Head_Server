using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelSummaryDTO>> GetAllSummariesAsync();
        Task<HotelDetailsDTO?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(HotelDetailsDTO dto);
        Task UpdateAsync(HotelDetailsDTO dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<HotelSummaryDTO>> GetByChainIdAsync(Guid chainId);
    }
}