using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IRoomPhotoService
    {
        Task<IEnumerable<RoomPhotoDTO>> GetByRoomIdAsync(Guid roomId);
        Task<RoomPhotoDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(RoomPhotoDTO dto);
        Task UpdateAsync(RoomPhotoDTO dto);
        Task DeleteAsync(Guid id);
        Task SetPreviewAsync(Guid photoId); 
    }
}

