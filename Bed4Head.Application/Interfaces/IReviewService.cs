using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetByHotelIdAsync(Guid hotelId);
        Task<ReviewDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(ReviewDTO dto);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(ReviewDTO dto);
    }
}

