using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
{
    public interface INearbyPlaceService
    {
        Task<IEnumerable<NearbyPlaceDTO>> GetAllAsync();
        Task<NearbyPlaceDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<NearbyPlaceDTO>> GetByHotelIdAsync(Guid hotelId);
        Task CreateAsync(NearbyPlaceDTO dto);
        Task UpdateAsync(NearbyPlaceDTO dto);
        Task DeleteAsync(Guid id);
    }
}