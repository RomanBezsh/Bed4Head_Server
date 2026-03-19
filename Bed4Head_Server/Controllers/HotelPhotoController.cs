using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head_Server.Controllers
{
    [Route("api/hotel-photos")]
    [ApiController]
    public class HotelPhotoController : ControllerBase
    {
        private readonly IHotelPhotoService _photoService;

        public HotelPhotoController(IHotelPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(Guid hotelId)
        {
            var photos = await _photoService.GetByHotelIdAsync(hotelId);
            return Ok(photos);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] HotelPhotoDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _photoService.CreateAsync(dto);
            return Ok(new { message = "Photo added successfully" });
        }

        [HttpPatch("{id}/set-primary")]
        public async Task<IActionResult> SetPrimary(Guid id)
        {
            var photo = await _photoService.GetByIdAsync(id);
            if (photo == null)
            {
                return NotFound(new { message = "Photo not found" });
            }

            await _photoService.SetPrimaryAsync(id);
            return Ok(new { message = "Primary photo updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _photoService.DeleteAsync(id);
            return Ok(new { message = "Photo deleted successfully" });
        }
    }
}