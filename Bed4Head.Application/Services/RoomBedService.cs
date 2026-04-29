using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Infrastructure.Repositories;

namespace Bed4Head.Application.Services
{
    public class RoomBedService : IRoomBedService
    {
        private readonly IUnitOfWork _db;

        public RoomBedService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RoomBedDTO>> GetByRoomIdAsync(Guid roomId)
        {
            var beds = await _db.RoomBeds.GetAllAsync();

            return beds
                .Where(b => b.RoomId == roomId)
                .Select(b => MapToDto(b));
        }

        public async Task<RoomBedDTO?> GetByIdAsync(Guid id)
        {
            var b = await _db.RoomBeds.GetByIdAsync(id);
            return b == null ? null : MapToDto(b);
        }

        public async Task CreateAsync(RoomBedDTO dto)
        {
            var bed = new RoomBed
            {
                Id = Guid.NewGuid(),
                Type = dto.Type,
                Count = dto.Count,
                RoomId = dto.RoomId
            };

            await _db.RoomBeds.AddAsync(bed);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(RoomBedDTO dto)
        {
            var bed = await _db.RoomBeds.GetByIdAsync(dto.Id);
            if (bed != null)
            {
                bed.Type = dto.Type;
                bed.Count = dto.Count;

                await _db.RoomBeds.UpdateAsync(bed);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.RoomBeds.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        private static RoomBedDTO MapToDto(RoomBed b) => new RoomBedDTO
        {
            Id = b.Id,
            Type = b.Type,
            Count = b.Count,
            RoomId = b.RoomId
        };
    }
}