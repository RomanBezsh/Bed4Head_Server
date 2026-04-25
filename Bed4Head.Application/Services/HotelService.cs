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
                Country = h.Country,
                Stars = h.Stars,
                HotelType = h.HotelType,
                BasePricePerNight = h.BasePricePerNight,
                CurrencyCode = h.CurrencyCode,
                OverallRating = h.OverallRating,
                RatingLabel = h.RatingLabel,
                ReviewsCount = h.ReviewsCount,
                IsFeatured = h.IsFeatured
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
                ShortDescription = h.ShortDescription,
                Stars = h.Stars,
                HotelType = h.HotelType,
                Address = h.Address,
                City = h.City,
                Country = h.Country,
                PostalCode = h.PostalCode,
                Latitude = h.Latitude,
                Longitude = h.Longitude,
                DistanceFromCenterKm = h.DistanceFromCenterKm,
                Phone = h.Phone,
                Email = h.Email,
                WebsiteUrl = h.WebsiteUrl,
                BasePricePerNight = h.BasePricePerNight,
                CurrencyCode = h.CurrencyCode,
                OverallRating = h.OverallRating,
                RatingLabel = h.RatingLabel,
                ReviewsCount = h.ReviewsCount,
                CheckInFrom = h.CheckInFrom,
                CheckOutUntil = h.CheckOutUntil,
                IsFeatured = h.IsFeatured,
                HotelChainId = h.HotelChainId
            };
        }
        public async Task<Guid> CreateAsync(HotelDetailsDTO dto)
        {
            var hotel = new Hotel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                ShortDescription = dto.ShortDescription,
                Stars = dto.Stars,
                HotelType = dto.HotelType ?? "Hotel",
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                DistanceFromCenterKm = dto.DistanceFromCenterKm,
                Phone = dto.Phone,
                Email = dto.Email,
                WebsiteUrl = dto.WebsiteUrl,
                BasePricePerNight = dto.BasePricePerNight,
                CurrencyCode = dto.CurrencyCode,
                OverallRating = dto.OverallRating,
                RatingLabel = dto.RatingLabel,
                ReviewsCount = dto.ReviewsCount,
                CheckInFrom = dto.CheckInFrom,
                CheckOutUntil = dto.CheckOutUntil,
                IsFeatured = dto.IsFeatured,
                HotelChainId = dto.HotelChainId
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
            hotel.ShortDescription = dto.ShortDescription;
            hotel.Stars = dto.Stars;
            hotel.HotelType = dto.HotelType ?? hotel.HotelType;
            hotel.Address = dto.Address;
            hotel.City = dto.City;
            hotel.Country = dto.Country;
            hotel.PostalCode = dto.PostalCode;
            hotel.Latitude = dto.Latitude;
            hotel.Longitude = dto.Longitude;
            hotel.DistanceFromCenterKm = dto.DistanceFromCenterKm;
            hotel.Phone = dto.Phone;
            hotel.Email = dto.Email;
            hotel.WebsiteUrl = dto.WebsiteUrl;
            hotel.BasePricePerNight = dto.BasePricePerNight;
            hotel.CurrencyCode = dto.CurrencyCode;
            hotel.OverallRating = dto.OverallRating;
            hotel.RatingLabel = dto.RatingLabel;
            hotel.ReviewsCount = dto.ReviewsCount;
            hotel.CheckInFrom = dto.CheckInFrom;
            hotel.CheckOutUntil = dto.CheckOutUntil;
            hotel.IsFeatured = dto.IsFeatured;
            hotel.HotelChainId = dto.HotelChainId;
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
                          Country = h.Country,
                          Stars = h.Stars,
                          HotelType = h.HotelType,
                          BasePricePerNight = h.BasePricePerNight,
                          CurrencyCode = h.CurrencyCode,
                          OverallRating = h.OverallRating,
                          RatingLabel = h.RatingLabel,
                          ReviewsCount = h.ReviewsCount,
                          IsFeatured = h.IsFeatured
                      });
        }
    }
}