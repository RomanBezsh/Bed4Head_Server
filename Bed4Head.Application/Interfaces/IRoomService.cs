using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDTO>> GetByHotelIdAsync(Guid hotelId);
        Task<RoomDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(RoomDTO dto);
        Task UpdateAsync(RoomDTO dto);
        Task DeleteAsync(Guid id);
    }
}

