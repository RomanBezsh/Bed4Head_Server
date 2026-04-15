using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IHotelPhotoService
    {
        Task<IEnumerable<HotelPhotoDTO>> GetAllAsync();
        Task<HotelPhotoDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<HotelPhotoDTO>> GetByHotelIdAsync(Guid hotelId);
        Task CreateAsync(HotelPhotoDTO dto);
        Task UpdateAsync(HotelPhotoDTO dto);
        Task SetPrimaryAsync(Guid photoId); 
        Task DeleteAsync(Guid id);
    }
}

