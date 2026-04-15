using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;

namespace Bed4Head.Application.Services
{
    public class NearbyPlaceService : INearbyPlaceService
    {
        private readonly IUnitOfWork _db;

        public NearbyPlaceService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<NearbyPlaceDTO>> GetAllAsync()
        {
            var places = await _db.NearbyPlaces.GetAllAsync();
            return places.Select(p => MapToDto(p));
        }

        public async Task<NearbyPlaceDTO?> GetByIdAsync(Guid id)
        {
            var p = await _db.NearbyPlaces.GetByIdAsync(id);
            return p == null ? null : MapToDto(p);
        }

        public async Task<IEnumerable<NearbyPlaceDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var all = await _db.NearbyPlaces.GetAllAsync();
            return all.Where(p => p.HotelId == hotelId)
                      .Select(p => MapToDto(p));
        }

        public async Task CreateAsync(NearbyPlaceDTO dto)
        {
            var place = new NearbyPlace
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                PlaceType = dto.PlaceType, // Исправлено
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                DistanceInMeters = dto.DistanceInMeters, // Исправлено
                HotelId = dto.HotelId
            };

            await _db.NearbyPlaces.AddAsync(place);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(NearbyPlaceDTO dto)
        {
            var place = await _db.NearbyPlaces.GetByIdAsync(dto.Id);
            if (place != null)
            {
                place.Name = dto.Name;
                place.PlaceType = dto.PlaceType; // Исправлено
                place.Latitude = dto.Latitude;
                place.Longitude = dto.Longitude;
                place.DistanceInMeters = dto.DistanceInMeters; // Исправлено

                await _db.NearbyPlaces.UpdateAsync(place);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.NearbyPlaces.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        private static NearbyPlaceDTO MapToDto(NearbyPlace p) => new NearbyPlaceDTO
        {
            Id = p.Id,
            Name = p.Name,
            PlaceType = p.PlaceType, 
            Latitude = p.Latitude,
            Longitude = p.Longitude,
            DistanceInMeters = p.DistanceInMeters, 
            HotelId = p.HotelId
        };
    }
}

