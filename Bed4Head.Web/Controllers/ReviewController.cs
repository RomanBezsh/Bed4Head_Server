using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bed4Head.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IHotelService _hotelService;
        private readonly IUserService _userService;

        public ReviewController(IReviewService reviewService, IHotelService hotelService, IUserService userService)
        {
            _reviewService = reviewService;
            _hotelService = hotelService;
            _userService = userService;
        }

        [HttpPost("hotels/{hotelId:guid}/reviews")]
        public async Task<IActionResult> Create(Guid hotelId, [FromBody] CreateReviewDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsScoreValid(dto.OverallScore))
            {
                return BadRequest(new { message = "OverallScore must be between 0 and 10" });
            }

            var hotel = await _hotelService.GetByIdAsync(hotelId);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            var user = await _userService.GetByIdAsync(dto.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            await _reviewService.CreateAsync(dto, hotelId);

            return Ok(new { message = "Review created successfully" });
        }

        [HttpGet("hotels/{hotelId:guid}/reviews")]
        public async Task<IActionResult> GetHotelReviews(Guid hotelId)
        {
            var hotel = await _hotelService.GetByIdAsync(hotelId);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            var reviews = await _reviewService.GetByHotelIdAsync(hotelId);
            return Ok(reviews);
        }

        [HttpGet("hotels/{hotelId:guid}/rating")]
        public async Task<IActionResult> GetHotelRating(Guid hotelId)
        {
            var hotel = await _hotelService.GetByIdAsync(hotelId);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            var rating = await _reviewService.GetHotelRatingAsync(hotelId);
            return Ok(rating);
        }

        [HttpGet("hotels/{hotelId:guid}/reviews/random")]
        public async Task<IActionResult> GetRandomHotelReviews(Guid hotelId, [FromQuery] int count = 5)
        {
            var hotel = await _hotelService.GetByIdAsync(hotelId);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            var reviews = await _reviewService.GetRandomByHotelIdAsync(hotelId, count);
            return Ok(reviews);
        }

        [HttpGet("reviews/random")]
        public async Task<IActionResult> GetRandomReviews([FromQuery] int count = 5)
        {
            var reviews = await _reviewService.GetRandomAsync(count);
            return Ok(reviews);
        }

        [HttpGet("reviews/random-hotel")]
        public async Task<IActionResult> GetRandomReviewsFromRandomHotel([FromQuery] int count = 5)
        {
            var reviews = await _reviewService.GetRandomFromRandomHotelAsync(count);
            return Ok(reviews);
        }

        [HttpGet("reviews/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            return Ok(review);
        }

        [HttpGet("reviews/me")]
        [Authorize]
        public async Task<IActionResult> GetMyReviews()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var reviews = await _reviewService.GetByUserIdAsync(userId.Value);
            return Ok(reviews);
        }

        [HttpPut("reviews/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReviewDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "Route id does not match body id" });
            }

            if (!IsScoreValid(dto.OverallScore))
            {
                return BadRequest(new { message = "OverallScore must be between 0 and 10" });
            }

            var existing = await _reviewService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            await _reviewService.UpdateAsync(dto);
            return Ok(new { message = "Review updated successfully" });
        }

        [HttpDelete("reviews/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _reviewService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            await _reviewService.DeleteAsync(id);
            return Ok(new { message = "Review deleted successfully" });
        }

        private static bool IsScoreValid(double score) => score >= 0 && score <= 10;

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                         ?? User.FindFirstValue("sub");

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }
    }
}
