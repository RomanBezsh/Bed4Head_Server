using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IRoomBedService
    {
        Task<IEnumerable<RoomBedDTO>> GetByRoomIdAsync(Guid roomId);
        Task<RoomBedDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(RoomBedDTO dto);
        Task UpdateAsync(RoomBedDTO dto);
        Task DeleteAsync(Guid id);
    }
}