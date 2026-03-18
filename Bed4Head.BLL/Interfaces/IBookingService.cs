using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
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