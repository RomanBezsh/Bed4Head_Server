using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head_Server.Controllers
{
    [Route("api/nearby-places")]
    [ApiController]
    public class NearbyPlaceController : ControllerBase
    {
        private readonly INearbyPlaceService _placeService;

        public NearbyPlaceController(INearbyPlaceService placeService)
        {
            _placeService = placeService;
        }

        // --- PUBLIC METHODS ---

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(Guid hotelId)
        {
            var places = await _placeService.GetByHotelIdAsync(hotelId);
            return Ok(places);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var place = await _placeService.GetByIdAsync(id);
            if (place == null)
            {
                return NotFound(new { message = "Nearby place not found" });
            }
            return Ok(place);
        }

        // --- ADMIN METHODS ---

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NearbyPlaceDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _placeService.CreateAsync(dto);
            return Ok(new { message = "Nearby place created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NearbyPlaceDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            await _placeService.UpdateAsync(dto);
            return Ok(new { message = "Nearby place updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _placeService.DeleteAsync(id);
            return Ok(new { message = "Nearby place deleted successfully" });
        }
    }
}