using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
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