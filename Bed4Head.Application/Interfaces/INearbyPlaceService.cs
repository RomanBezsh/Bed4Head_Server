using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
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

