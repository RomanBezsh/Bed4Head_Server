using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head_Server.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // --- PUBLIC METHODS ---

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(Guid hotelId)
        {
            var reviews = await _reviewService.GetByHotelIdAsync(hotelId);
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }
            return Ok(review);
        }

        // --- USER METHODS ---

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ReviewDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Сервис сам проставит CreatedAt = DateTime.UtcNow
            await _reviewService.CreateAsync(dto);
            return Ok(new { message = "Review submitted successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReviewDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            await _reviewService.UpdateAsync(dto);
            return Ok(new { message = "Review updated successfully" });
        }

        // --- ADMIN / USER METHODS ---

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _reviewService.DeleteAsync(id);
            return Ok(new { message = "Review deleted successfully" });
        }
    }
}