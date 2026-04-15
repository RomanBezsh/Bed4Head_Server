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
                Price = dto.Price,
                BedType = dto.BedType,
                MaxGuests = dto.MaxGuests,
                FreeCancellation = dto.FreeCancellation,
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
                room.Price = dto.Price;
                room.BedType = dto.BedType;
                room.MaxGuests = dto.MaxGuests;
                room.FreeCancellation = dto.FreeCancellation;

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
            Price = r.Price,
            BedType = r.BedType,
            MaxGuests = r.MaxGuests,
            FreeCancellation = r.FreeCancellation,
            HotelId = r.HotelId,
        };
    }
}

