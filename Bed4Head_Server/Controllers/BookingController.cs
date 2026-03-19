using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head_Server.Controllers
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BookingDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookingService.CreateAsync(dto);

            return Ok(new { message = "Booking created successfully" });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings(Guid userId)
        {
            var bookings = await _bookingService.GetByUserIdAsync(userId);
            return Ok(bookings);
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _bookingService.DeleteAsync(id);
            return Ok(new { message = "Booking cancelled successfully" });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }
    }
}