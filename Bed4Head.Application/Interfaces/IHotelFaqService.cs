using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
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

