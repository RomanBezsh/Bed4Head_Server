using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;
namespace Bed4Head.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _db;
        public RoomService(IUnitOfWork db)
        {
            _db = db;
        }
        public async Task<IEnumerable<RoomDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var rooms = await _db.Rooms.GetAllAsync();
            return rooms.Where(r => r.HotelId == hotelId)
                        .Select(r => MapToDto(r));
        }
        public async Task<RoomDTO?> GetByIdAsync(Guid id)
        {
            var r = await _db.Rooms.GetByIdAsync(id);
            return r == null ? null : MapToDto(r);
        }
        public async Task CreateAsync(RoomDTO dto)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                CurrencyCode = dto.CurrencyCode,
                BedType = dto.BedType,
                RoomType = dto.RoomType,
                View = dto.View,
                AreaInSquareMeters = dto.AreaInSquareMeters,
                MaxGuests = dto.MaxGuests,
                AvailableUnits = dto.AvailableUnits,
                FreeCancellation = dto.FreeCancellation,
                BreakfastIncluded = dto.BreakfastIncluded,
                PrivateBathroom = dto.PrivateBathroom,
                HotelId = dto.HotelId
            };
            await _db.Rooms.AddAsync(room);
            await _db.CompleteAsync();
        }
        public async Task UpdateAsync(RoomDTO dto)
        {
            var room = await _db.Rooms.GetByIdAsync(dto.Id);
            if (room != null)
            {
                room.Title = dto.Title;
                room.Description = dto.Description;
                room.Price = dto.Price;
                room.CurrencyCode = dto.CurrencyCode;
                room.BedType = dto.BedType;
                room.RoomType = dto.RoomType;
                room.View = dto.View;
                room.AreaInSquareMeters = dto.AreaInSquareMeters;
                room.MaxGuests = dto.MaxGuests;
                room.AvailableUnits = dto.AvailableUnits;
                room.FreeCancellation = dto.FreeCancellation;
                room.BreakfastIncluded = dto.BreakfastIncluded;
                room.PrivateBathroom = dto.PrivateBathroom;
                await _db.Rooms.UpdateAsync(room);
                await _db.CompleteAsync();
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            await _db.Rooms.DeleteAsync(id);
            await _db.CompleteAsync();
        }
        private static RoomDTO MapToDto(Room r) => new RoomDTO
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            Price = r.Price,
            CurrencyCode = r.CurrencyCode,
            BedType = r.BedType,
            RoomType = r.RoomType,
            View = r.View,
            AreaInSquareMeters = r.AreaInSquareMeters,
            MaxGuests = r.MaxGuests,
            AvailableUnits = r.AvailableUnits,
            FreeCancellation = r.FreeCancellation,
            BreakfastIncluded = r.BreakfastIncluded,
            PrivateBathroom = r.PrivateBathroom,
            HotelId = r.HotelId,
        };
    }
}