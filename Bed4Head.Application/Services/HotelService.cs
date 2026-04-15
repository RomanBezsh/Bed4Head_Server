using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;

namespace Bed4Head.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _db;

        public HotelService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelSummaryDTO>> GetAllSummariesAsync()
        {
            var hotels = await _db.Hotels.GetAllAsync();
            return hotels.Select(h => new HotelSummaryDTO
            {
                Id = h.Id,
                Name = h.Name,
                City = h.City,
                Stars = h.Stars,
                OverallRating = h.OverallRating,
                ReviewsCount = h.ReviewsCount
            });
        }

        public async Task<HotelDetailsDTO?> GetByIdAsync(Guid id)
        {
            var h = await _db.Hotels.GetByIdAsync(id);
            if (h == null) return null;

            return new HotelDetailsDTO
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                Stars = h.Stars,
                Address = h.Address,
                City = h.City,
                Latitude = h.Latitude,
                Longitude = h.Longitude,
                Phone = h.Phone,
                Email = h.Email,
                HotelType = h.HotelType,
                HotelChainId = h.HotelChainId,
                OverallRating = h.OverallRating,
                ReviewsCount = h.ReviewsCount
            };
        }

        public async Task<Guid> CreateAsync(HotelDetailsDTO dto)
        {
            var hotel = new Hotel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                Stars = dto.Stars,
                Address = dto.Address,
                City = dto.City,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Phone = dto.Phone,
                Email = dto.Email,
                HotelType = dto.HotelType ?? "Hotel",
                HotelChainId = dto.HotelChainId,
                OverallRating = dto.OverallRating,
                ReviewsCount = dto.ReviewsCount
            };

            await _db.Hotels.AddAsync(hotel);
            await _db.CompleteAsync();
            return hotel.Id;
        }

        public async Task UpdateAsync(HotelDetailsDTO dto)
        {
            var hotel = await _db.Hotels.GetByIdAsync(dto.Id);
            if (hotel == null) return;

            hotel.Name = dto.Name;
            hotel.Description = dto.Description ?? string.Empty;
            hotel.Stars = dto.Stars;
            hotel.Address = dto.Address;
            hotel.City = dto.City;
            hotel.Latitude = dto.Latitude;
            hotel.Longitude = dto.Longitude;
            hotel.Phone = dto.Phone;
            hotel.Email = dto.Email;
            hotel.HotelType = dto.HotelType;
            hotel.HotelChainId = dto.HotelChainId;
            hotel.OverallRating = dto.OverallRating;
            hotel.ReviewsCount = dto.ReviewsCount;

            await _db.Hotels.UpdateAsync(hotel);
            await _db.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Hotels.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        public async Task<IEnumerable<HotelSummaryDTO>> GetByChainIdAsync(Guid chainId)
        {
            var all = await _db.Hotels.GetAllAsync();

            return all.Where(h => h.HotelChainId == chainId)
                      .Select(h => new HotelSummaryDTO
                      {
                          Id = h.Id,
                          Name = h.Name,
                          City = h.City,
                          Stars = h.Stars,
                          OverallRating = h.OverallRating,
                          ReviewsCount = h.ReviewsCount
                      });
        }
    }
}

