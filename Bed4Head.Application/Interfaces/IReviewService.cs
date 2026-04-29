using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetByHotelIdAsync(Guid hotelId);
        Task<IEnumerable<ReviewDTO>> GetRandomAsync(int count);
        Task<IEnumerable<ReviewDTO>> GetRandomByHotelIdAsync(Guid hotelId, int count);
        Task<IEnumerable<ReviewDTO>> GetRandomFromRandomHotelAsync(int count);
        Task<HotelRatingDTO> GetHotelRatingAsync(Guid hotelId);
        Task<ReviewDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(CreateReviewDTO dto, Guid hotelId);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(ReviewDTO dto);
    }
}

