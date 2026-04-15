using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{
    [Route("api/hotel-chains")]
    [ApiController]
    public class HotelChainController : ControllerBase
    {
        private readonly IHotelChainService _hotelChainService;

        public HotelChainController(IHotelChainService hotelChainService)
        {
            _hotelChainService = hotelChainService;
        }

        // --- PUBLIC METHODS ---

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chains = await _hotelChainService.GetAllAsync();
            return Ok(chains);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var chain = await _hotelChainService.GetByIdAsync(id);
            if (chain == null)
            {
                return NotFound(new { message = "Hotel chain not found" });
            }
            return Ok(chain);
        }

        // --- ADMIN METHODS ---

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] HotelChainDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _hotelChainService.CreateAsync(dto);

            return Ok(new { message = "Hotel chain created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] HotelChainDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            await _hotelChainService.UpdateAsync(dto);
            return Ok(new { message = "Hotel chain updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _hotelChainService.DeleteAsync(id);
            return Ok(new { message = "Hotel chain deleted successfully" });
        }
    }
}


