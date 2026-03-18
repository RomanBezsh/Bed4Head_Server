using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
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