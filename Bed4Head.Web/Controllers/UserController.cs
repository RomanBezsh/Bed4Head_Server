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
        private readonly IWebHostEnvironment _env;

        public UserController(IUserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;
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

        [HttpPost("{userId}/upload-avatar")]
        public async Task<IActionResult> UploadAvatar(Guid userId, [FromForm] IFormFile file)
        {
            var result = await UploadImageHelper(file, "avatars");
            
            if (result is OkObjectResult okResult)
            {
                // Извлекаем URL из анонимного объекта { url = "..." }
                var url = okResult.Value?.GetType().GetProperty("url")?.GetValue(okResult.Value)?.ToString();
                if (!string.IsNullOrEmpty(url))
                {
                    await _userService.UpdateAvatarAsync(userId, url);
                }
            }

            return result;
        }

        private async Task<IActionResult> UploadImageHelper(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is empty" });

            string webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            string uploadsFolder = Path.Combine(webRoot, "uploads", subFolder);
            
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var fileUrl = $"{baseUrl}/uploads/{subFolder}/{uniqueFileName}";

            return Ok(new { url = fileUrl });
        }
    }
}


