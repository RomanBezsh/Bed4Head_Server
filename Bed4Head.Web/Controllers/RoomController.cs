using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // --- PUBLIC METHODS ---

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(Guid hotelId)
        {
            var rooms = await _roomService.GetByHotelIdAsync(hotelId);
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound(new { message = "Room not found" });
            }
            return Ok(room);
        }

        // --- ADMIN METHODS ---

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoomDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _roomService.CreateAsync(dto);
            return Ok(new { message = "Room created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoomDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            await _roomService.UpdateAsync(dto);
            return Ok(new { message = "Room updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roomService.DeleteAsync(id);
            return Ok(new { message = "Room deleted successfully" });
        }
    }
}


