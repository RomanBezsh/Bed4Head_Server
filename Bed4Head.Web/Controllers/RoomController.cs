using Bed4Head.Application.DTOs;
using Bed4Head.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Bed4Head.Infrastructure.Data;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Web.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public RoomController(IUnitOfWork db, IWebHostEnvironment env, AppDbContext context)
        {
            _db = db;
            _env = env;
            _context = context;
        }
        [HttpPost]
[Authorize(Policy = "AdminOnly")]
[RequestSizeLimit(50_000_000)]
public async Task<IActionResult> Create([FromForm] CreateRoomRequestDTO request)
{
    try
    {
        // ✅ Валидация
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Title is required" });

        if (request.Price <= 0)
            return BadRequest(new { message = "Price must be greater than 0" });

        if (request.MaxGuests <= 0)
            return BadRequest(new { message = "MaxGuests must be greater than 0" });

        // ✅ Проверка отеля
        var hotel = await _db.Hotels.GetByIdAsync(request.HotelId);
        if (hotel == null)
            return BadRequest(new { message = "Hotel not found" });

        // ✅ Создание комнаты
        var room = new Room
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Price = request.Price,
            CurrencyCode = request.CurrencyCode,
            MaxGuests = request.MaxGuests,
            HotelId = request.HotelId,

            FreeCancellation = request.FreeCancellation,
            PrivateBathroom = request.PrivateBathroom,
            HasWifi = request.HasWifi,
            HasPrivatePool = request.HasPrivatePool
        };

        await _db.Rooms.AddAsync(room);

        // ✅ КРОВАТИ
        if (!string.IsNullOrWhiteSpace(request.Beds))
        {
            var beds = JsonSerializer.Deserialize<List<RoomBedDTO>>(
                request.Beds,
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
            );

            if (beds != null)
            {
                foreach (var bed in beds)
                {
                    if (string.IsNullOrWhiteSpace(bed.Type) || bed.Count <= 0)
                        continue;

                    await _db.RoomBeds.AddAsync(new RoomBed
                    {
                        Id = Guid.NewGuid(),
                        Type = bed.Type,
                        Count = bed.Count,
                        RoomId = room.Id
                    });
                }
            }
        }

        // ✅ ФОТО
        string? previewUrl = null;

        if (request.PreviewImage != null)
        {
            previewUrl = await SaveRoomPhoto(request.PreviewImage);

            await _db.RoomPhotos.AddAsync(new RoomPhoto
            {
                Id = Guid.NewGuid(),
                Url = previewUrl!,
                RoomId = room.Id,
                IsPreview = true
            });
        }

        // ✅ 1. Сохраняем комнату
        await _db.CompleteAsync();

        // ✅ 2. Считаем медиану цен комнат
        await UpdateHotelMedianPriceAsync(request.HotelId);

        // ✅ 3. Сохраняем обновлённый отель
        await _db.CompleteAsync();

        // ✅ Ответ
        return Ok(new RoomDTO
        {
            Id = room.Id,
            Title = room.Title,
            Price = room.Price,
            CurrencyCode = room.CurrencyCode,
            MaxGuests = room.MaxGuests,
            FreeCancellation = room.FreeCancellation,
            PrivateBathroom = room.PrivateBathroom,
            HasWifi = room.HasWifi,
            HasPrivatePool = room.HasPrivatePool,
            HotelId = room.HotelId,
            PreviewImage = previewUrl,
            Beds = new List<RoomBedDTO>()
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = "Server error",
            detail = ex.Message
        });
    }
}

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] Guid? hotelId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int? guests)
        {
            var roomsQuery = _context.Rooms
                .AsNoTracking()
                .Include(r => r.Beds)
                .Include(r => r.Photos)
                .AsQueryable();

            if (hotelId.HasValue && hotelId.Value != Guid.Empty)
            {
                roomsQuery = roomsQuery.Where(r => r.HotelId == hotelId.Value);
            }

            if (guests.HasValue && guests.Value > 0)
            {
                roomsQuery = roomsQuery.Where(r => r.MaxGuests >= guests.Value);
            }

            if (from.HasValue && to.HasValue && from.Value.Date < to.Value.Date)
            {
                var checkIn = from.Value.Date;
                var checkOut = to.Value.Date;
                var bookedRoomIds = _context.Bookings
                    .AsNoTracking()
                    .Where(b =>
                        b.CheckIn.Date < checkOut &&
                        b.CheckOut.Date > checkIn &&
                        (b.Status == null ||
                         (b.Status.ToLower() != "cancelled" && b.Status.ToLower() != "canceled")))
                    .Select(b => b.RoomId);

                roomsQuery = roomsQuery.Where(r => !bookedRoomIds.Contains(r.Id));
            }

            var rooms = await roomsQuery
                .OrderBy(r => r.Title)
                .ToListAsync();

            var result = rooms.Select(ToRoomDto).ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .Include(r => r.Beds)
                .Include(r => r.Photos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
                return NotFound(new { message = "Room not found" });

            return Ok(ToRoomDto(room));
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateRoomRequestDTO request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                    return BadRequest(new { message = "Title is required" });

                if (request.Price <= 0)
                    return BadRequest(new { message = "Price must be greater than 0" });

                if (request.MaxGuests <= 0)
                    return BadRequest(new { message = "MaxGuests must be greater than 0" });

                var room = await _context.Rooms
                    .Include(r => r.Beds)
                    .Include(r => r.Photos)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (room == null)
                    return NotFound(new { message = "Room not found" });

                var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == request.HotelId);
                if (hotel == null)
                    return BadRequest(new { message = "Hotel not found" });

                var previousHotelId = room.HotelId;

                room.Title = request.Title;
                room.Price = request.Price;
                room.CurrencyCode = request.CurrencyCode;
                room.MaxGuests = request.MaxGuests;
                room.HotelId = request.HotelId;
                room.FreeCancellation = request.FreeCancellation;
                room.PrivateBathroom = request.PrivateBathroom;
                room.HasWifi = request.HasWifi;
                room.HasPrivatePool = request.HasPrivatePool;

                _context.RoomBeds.RemoveRange(room.Beds);
                if (!string.IsNullOrWhiteSpace(request.Beds))
                {
                    var beds = JsonSerializer.Deserialize<List<RoomBedDTO>>(
                        request.Beds,
                        new JsonSerializerOptions(JsonSerializerDefaults.Web)
                    );

                    foreach (var bed in beds ?? [])
                    {
                        if (string.IsNullOrWhiteSpace(bed.Type) || bed.Count <= 0)
                            continue;

                        room.Beds.Add(new RoomBed
                        {
                            Id = Guid.NewGuid(),
                            Type = bed.Type,
                            Count = bed.Count,
                            RoomId = room.Id
                        });
                    }
                }

                if (request.PreviewImage != null)
                {
                    foreach (var photo in room.Photos.Where(p => p.IsPreview.GetValueOrDefault()))
                    {
                        photo.IsPreview = false;
                    }

                    var previewUrl = await SaveRoomPhoto(request.PreviewImage);
                    if (!string.IsNullOrWhiteSpace(previewUrl))
                    {
                        room.Photos.Add(new RoomPhoto
                        {
                            Id = Guid.NewGuid(),
                            Url = previewUrl,
                            RoomId = room.Id,
                            IsPreview = true
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await UpdateHotelMedianPriceAsync(request.HotelId);

                if (previousHotelId != request.HotelId)
                {
                    await UpdateHotelMedianPriceAsync(previousHotelId);
                }

                var updatedRoom = await _context.Rooms
                    .AsNoTracking()
                    .Include(r => r.Beds)
                    .Include(r => r.Photos)
                    .FirstAsync(r => r.Id == id);

                return Ok(ToRoomDto(updatedRoom));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Server error",
                    detail = ex.Message
                });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var room = await _context.Rooms
                .Include(r => r.Beds)
                .Include(r => r.Photos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
                return NotFound(new { message = "Room not found" });

            var hotelId = room.HotelId;
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            await UpdateHotelMedianPriceAsync(hotelId);

            return Ok(new { message = "Room deleted successfully" });
        }
        // =====================

        private static RoomDTO ToRoomDto(Room room)
        {
            var preview = room.Photos
                .OrderByDescending(p => p.IsPreview.GetValueOrDefault())
                .ThenBy(p => p.Id)
                .FirstOrDefault();

            return new RoomDTO
            {
                Id = room.Id,
                Title = room.Title,
                Price = room.Price,
                CurrencyCode = room.CurrencyCode,
                MaxGuests = room.MaxGuests,
                FreeCancellation = room.FreeCancellation,
                PrivateBathroom = room.PrivateBathroom,
                HasWifi = room.HasWifi,
                HasPrivatePool = room.HasPrivatePool,
                HotelId = room.HotelId,
                PreviewImage = preview?.Url,
                Beds = room.Beds
                    .Select(b => new RoomBedDTO
                    {
                        Type = b.Type,
                        Count = b.Count
                    })
                    .ToList()
            };
        }

        private async Task UpdateHotelMedianPriceAsync(Guid hotelId)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId);
            if (hotel == null)
            {
                return;
            }

            var prices = await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .Select(r => r.Price)
                .OrderBy(p => p)
                .ToListAsync();

            if (prices.Count == 0)
            {
                hotel.BasePricePerNight = 0;
            }
            else
            {
                var middle = prices.Count / 2;
                hotel.BasePricePerNight = prices.Count % 2 == 0
                    ? (prices[middle - 1] + prices[middle]) / 2m
                    : prices[middle];
            }

            await _context.SaveChangesAsync();
        }

        private async Task<string?> SaveRoomPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var folder = Path.Combine(webRoot, "uploads", "rooms");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(folder, fileName);

            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/rooms/{fileName}";
        }
    }
}
