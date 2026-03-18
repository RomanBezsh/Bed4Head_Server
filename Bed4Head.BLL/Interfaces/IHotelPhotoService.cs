using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
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