using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDTO>> GetAllAsync();
        Task<BookingDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<BookingDTO>> GetByUserIdAsync(Guid userId);
        Task CreateAsync(CreateBookingDTO dto, Guid userId);
        Task UpdateStatusAsync(Guid id, string status);
        Task CancelAsync(Guid id);
    }
}

