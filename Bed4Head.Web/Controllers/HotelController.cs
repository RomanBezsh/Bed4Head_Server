using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // --- PUBLIC METHODS ---

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hotels = await _hotelService.GetAllSummariesAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }
            return Ok(hotel);
        }

        [HttpGet("chain/{chainId}")]
        public async Task<IActionResult> GetByChain(Guid chainId)
        {
            var hotels = await _hotelService.GetByChainIdAsync(chainId);
            return Ok(hotels);
        }

        // --- ADMIN METHODS ---

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] HotelDetailsDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newHotelId = await _hotelService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = newHotelId }, new { id = newHotelId, message = "Hotel created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] HotelDetailsDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            await _hotelService.UpdateAsync(dto);
            return Ok(new { message = "Hotel updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _hotelService.DeleteAsync(id);
            return Ok(new { message = "Hotel deleted successfully" });
        }
    }
}


