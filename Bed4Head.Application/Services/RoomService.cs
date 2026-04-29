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

        // ✅ Получить комнаты отеля
        public async Task<IEnumerable<RoomDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var rooms = (await _db.Rooms.GetAllAsync())
                .Where(r => r.HotelId == hotelId)
                .ToList();

            var allBeds = await _db.RoomBeds.GetAllAsync();
            var allPhotos = await _db.RoomPhotos.GetAllAsync();

            return rooms.Select(room =>
            {
                var dto = MapToDto(room);

                dto.Beds = allBeds
                    .Where(b => b.RoomId == room.Id)
                    .Select(b => new RoomBedDTO
                    {
                        Id = b.Id,
                        Type = b.Type,
                        Count = b.Count,
                        RoomId = b.RoomId
                    })
                    .ToList();

                dto.PreviewImage = allPhotos
                    .Where(p => p.RoomId == room.Id)
                    .Select(p => p.Url)
                    .FirstOrDefault(); // 👈 одно фото

                return dto;
            });
        }

        // ✅ Получить одну комнату
        public async Task<RoomDTO?> GetByIdAsync(Guid id)
        {
            var room = await _db.Rooms.GetByIdAsync(id);
            if (room == null) return null;

            var dto = MapToDto(room);

            var beds = await _db.RoomBeds.GetAllAsync();
            dto.Beds = beds
                .Where(b => b.RoomId == room.Id)
                .Select(b => new RoomBedDTO
                {
                    Id = b.Id,
                    Type = b.Type,
                    Count = b.Count,
                    RoomId = b.RoomId
                })
                .ToList();

            var photos = await _db.RoomPhotos.GetAllAsync();
            dto.PreviewImage = photos
                .Where(p => p.RoomId == room.Id)
                .Select(p => p.Url)
                .FirstOrDefault();

            return dto;
        }

        // ✅ Создать комнату
        public async Task CreateAsync(RoomDTO dto)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Price = dto.Price,
                CurrencyCode = dto.CurrencyCode,
                MaxGuests = dto.MaxGuests,
                FreeCancellation = dto.FreeCancellation,
                PrivateBathroom = dto.PrivateBathroom,
                HasWifi = dto.HasWifi,
                HasPrivatePool = dto.HasPrivatePool,
                HotelId = dto.HotelId
            };

            await _db.Rooms.AddAsync(room);

            // ✅ КРОВАТИ
            if (dto.Beds != null && dto.Beds.Any())
            {
                foreach (var bed in dto.Beds)
                {
                    await _db.RoomBeds.AddAsync(new RoomBed
                    {
                        Id = Guid.NewGuid(),
                        Type = bed.Type,
                        Count = bed.Count,
                        RoomId = room.Id
                    });
                }
            }

            // ✅ ПРЕВЬЮ ФОТО (одно)
            if (!string.IsNullOrWhiteSpace(dto.PreviewImage))
            {
                await _db.RoomPhotos.AddAsync(new RoomPhoto
                {
                    Id = Guid.NewGuid(),
                    Url = dto.PreviewImage,
                    RoomId = room.Id
                });
            }

            await _db.CompleteAsync();
        }

        // ✅ Обновить комнату
        public async Task UpdateAsync(RoomDTO dto)
        {
            var room = await _db.Rooms.GetByIdAsync(dto.Id);
            if (room == null) return;

            room.Title = dto.Title;
            room.Price = dto.Price;
            room.CurrencyCode = dto.CurrencyCode;
            room.MaxGuests = dto.MaxGuests;
            room.FreeCancellation = dto.FreeCancellation;
            room.PrivateBathroom = dto.PrivateBathroom;
            room.HasWifi = dto.HasWifi;
            room.HasPrivatePool = dto.HasPrivatePool;

            await _db.Rooms.UpdateAsync(room);

            // ❗ УДАЛЯЕМ СТАРЫЕ КРОВАТИ
            var existingBeds = await _db.RoomBeds.GetAllAsync();
            foreach (var bed in existingBeds.Where(b => b.RoomId == room.Id))
            {
                await _db.RoomBeds.DeleteAsync(bed.Id);
            }

            // ❗ ДОБАВЛЯЕМ НОВЫЕ
            if (dto.Beds != null)
            {
                foreach (var bed in dto.Beds)
                {
                    await _db.RoomBeds.AddAsync(new RoomBed
                    {
                        Id = Guid.NewGuid(),
                        Type = bed.Type,
                        Count = bed.Count,
                        RoomId = room.Id
                    });
                }
            }

            // ❗ УДАЛЯЕМ СТАРОЕ ФОТО
            var existingPhotos = await _db.RoomPhotos.GetAllAsync();
            foreach (var photo in existingPhotos.Where(p => p.RoomId == room.Id))
            {
                await _db.RoomPhotos.DeleteAsync(photo.Id);
            }

            // ❗ ДОБАВЛЯЕМ НОВОЕ
            if (!string.IsNullOrWhiteSpace(dto.PreviewImage))
            {
                await _db.RoomPhotos.AddAsync(new RoomPhoto
                {
                    Id = Guid.NewGuid(),
                    Url = dto.PreviewImage,
                    RoomId = room.Id
                });
            }

            await _db.CompleteAsync();
        }

        // ✅ Удалить
        public async Task DeleteAsync(Guid id)
        {
            await _db.Rooms.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        // 🔧 Маппер
        private static RoomDTO MapToDto(Room r) => new RoomDTO
        {
            Id = r.Id,
            Title = r.Title,
            Price = r.Price,
            CurrencyCode = r.CurrencyCode,
            MaxGuests = r.MaxGuests,
            FreeCancellation = r.FreeCancellation,
            PrivateBathroom = r.PrivateBathroom,
            HasWifi = r.HasWifi,
            HasPrivatePool = r.HasPrivatePool,
            HotelId = r.HotelId,

            Beds = new List<RoomBedDTO>(),
            PreviewImage = null
        };
    }
}