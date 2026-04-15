using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDTO dto)
        {
            var user = await _userService.GetByIdAsync(dto.Id);
            if (user == null) return NotFound();

            await _userService.UpdateAsync(dto);
            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpPatch("{id}/preferences")]
        public async Task<IActionResult> UpdatePreferences(Guid id, [FromQuery] bool seasonal, [FromQuery] bool favorite)
        {
            await _userService.UpdateNewsPreferencesAsync(id, seasonal, favorite, true, true);
            return Ok();
        }
    }
}


