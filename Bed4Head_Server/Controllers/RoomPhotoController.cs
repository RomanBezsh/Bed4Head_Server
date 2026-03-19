using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head_Server.Controllers
{
    [Route("api/room-photos")]
    [ApiController]
    public class RoomPhotoController : ControllerBase
    {
        private readonly IRoomPhotoService _roomPhotoService;

        public RoomPhotoController(IRoomPhotoService roomPhotoService)
        {
            _roomPhotoService = roomPhotoService;
        }

        // --- PUBLIC METHODS ---

        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetByRoom(Guid roomId)
        {
            var photos = await _roomPhotoService.GetByRoomIdAsync(roomId);
            return Ok(photos);
        }

        // --- ADMIN METHODS ---

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoomPhotoDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _roomPhotoService.CreateAsync(dto);
            return Ok(new { message = "Room photo added successfully" });
        }

        [HttpPatch("{id}/set-preview")]
        public async Task<IActionResult> SetPreview(Guid id)
        {
            var photo = await _roomPhotoService.GetByIdAsync(id);
            if (photo == null)
            {
                return NotFound(new { message = "Photo not found" });
            }

            await _roomPhotoService.SetPreviewAsync(id);
            return Ok(new { message = "Room preview updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roomPhotoService.DeleteAsync(id);
            return Ok(new { message = "Room photo deleted successfully" });
        }
    }
}