using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{
    [Route("api/hotel-faqs")]
    [ApiController]
    public class HotelFaqController : ControllerBase
    {
        private readonly IHotelFaqService _faqService;

        public HotelFaqController(IHotelFaqService faqService)
        {
            _faqService = faqService;
        }

        // --- PUBLIC METHODS ---

        // Get all FAQs for a specific hotel
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(Guid hotelId)
        {
            var faqs = await _faqService.GetByHotelIdAsync(hotelId);
            return Ok(faqs);
        }

        // --- ADMIN METHODS ---

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] HotelFaqDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _faqService.CreateAsync(dto);
            return Ok(new { message = "FAQ created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] HotelFaqDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            await _faqService.UpdateAsync(dto);
            return Ok(new { message = "FAQ updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _faqService.DeleteAsync(id);
            return Ok(new { message = "FAQ deleted successfully" });
        }
    }
} 


