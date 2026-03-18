using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
{
    public interface IHotelFaqService
    {
        Task<IEnumerable<HotelFaqDTO>> GetByHotelIdAsync(Guid hotelId);
        Task<HotelFaqDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(HotelFaqDTO dto);
        Task UpdateAsync(HotelFaqDTO dto);
        Task DeleteAsync(Guid id);
    }
}