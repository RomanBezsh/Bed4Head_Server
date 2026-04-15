using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDTO>> GetAllAsync();
        Task<BookingDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<BookingDTO>> GetByUserIdAsync(Guid userId);
        Task CreateAsync(BookingDTO dto);
        Task UpdateStatusAsync(Guid id, string status);
        Task DeleteAsync(Guid id);
    }
}

