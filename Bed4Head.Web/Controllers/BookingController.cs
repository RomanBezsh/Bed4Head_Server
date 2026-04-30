using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bed4Head.Web.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // Создание бронирования
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateBookingDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                         ?? User.FindFirstValue("sub");

            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            await _bookingService.CreateAsync(dto, parsedUserId);

            return Ok(new { message = "Booking created successfully" });
        }
        
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("WORKS");
        }

        // Получить все бронирования пользователя
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings(Guid userId)
        {
            var bookings = await _bookingService.GetByUserIdAsync(userId);
            return Ok(bookings);
        }

        // Получить одно бронирование
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        // Отмена (меняем статус, а не удаляем)
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _bookingService.CancelAsync(id);
            return Ok(new { message = "Booking cancelled successfully" });
        }

        // Изменение статуса (например админом)
        [HttpPatch("{id}/status")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] string status)
        {
            await _bookingService.UpdateStatusAsync(id, status);
            return Ok(new { message = "Status updated successfully" });
        }

        // Все бронирования (админ)
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }
    }
}
