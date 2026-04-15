using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{
    [Route("api/amenities")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenities = await _amenityService.GetAllAsync();
            return Ok(amenities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var amenity = await _amenityService.GetByIdAsync(id);
            if (amenity == null) return NotFound();
            return Ok(amenity);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AmenityDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _amenityService.CreateAsync(dto);
            return Ok(new { message = "Amenity created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AmenityDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            await _amenityService.UpdateAsync(dto);
            return Ok(new { message = "Amenity updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _amenityService.DeleteAsync(id);
            return Ok(new { message = "Amenity removed" });
        }
    }
}


