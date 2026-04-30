using Bed4Head.Application.DTOs;
using Bed4Head.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Bed4Head.Infrastructure.Repositories;

namespace Bed4Head.Web.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IWebHostEnvironment _env;

        public RoomController(IUnitOfWork db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
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
                var hotelExists = await _db.Hotels.GetByIdAsync(request.HotelId);
                if (hotelExists == null)
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

                // ✅ КРОВАТИ (исправлено)
                if (!string.IsNullOrWhiteSpace(request.Beds))
                {
                    var beds = JsonSerializer.Deserialize<List<RoomBedDTO>>(
                        request.Beds,
                        new JsonSerializerOptions(JsonSerializerDefaults.Web) // 🔥 фикс
                    );

                    if (beds != null)
                    {
                        foreach (var bed in beds)
                        {
                            // 🔒 защита от null
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
                    Beds = new List<RoomBedDTO>() // при желании можно вернуть реальные
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
        public async Task<IActionResult> GetByHotelId([FromQuery] Guid hotelId)
        {
            var rooms = (await _db.Rooms.GetAllAsync())
                .Where(r => r.HotelId == hotelId)
                .ToList();
            var result = new List<RoomDTO>();

            foreach (var room in rooms)
            {
                var beds = (await _db.RoomBeds.GetAllAsync())
                    .Where(b => b.RoomId == room.Id)
                    .ToList();

                var preview = (await _db.RoomPhotos.GetAllAsync())
                    .Where(p => p.RoomId == room.Id && p.IsPreview.GetValueOrDefault())
                    .ToList();
                
                result.Add(new RoomDTO
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

                    PreviewImage = preview.FirstOrDefault()?.Url,

                    Beds = beds?.Select(b => new RoomBedDTO
                    {
                        Type = b.Type,
                        Count = b.Count
                    }).ToList() ?? new List<RoomBedDTO>()
                });
            }

            return Ok(result);
        }
        // =====================

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