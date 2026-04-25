using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IWebHostEnvironment _env;

        public AuthController(IUserService userService, IAuthService authService, IWebHostEnvironment env)
        {
            _userService = userService;
            _authService = authService;
            _env = env;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);

            if (result == null)
                return BadRequest("User already exists or registration failed");

            if (_env.IsDevelopment())
            {
                return Ok(new
                {
                    Message = "Registration successful. If email sending is disabled, check server logs (confirmation codes are not stored in DB).",
                    User = result,
                });
            }

            return Ok(new
            {
                Message = "Registration successful. Check your email for confirmation code.",
                User = result
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto) // ???????? Query ?? Body
        {
            bool isValid = await _authService.VerifyPasswordAsync(dto.Email, dto.Password);
            if (!isValid) return Unauthorized("Invalid email or password");

            var userDto = await _userService.GetByEmailAsync(dto.Email);
            if (userDto == null) return NotFound();

            if (!userDto.IsEmailConfirmed)
                return BadRequest("Email not confirmed. Please confirm your email first.");

            var token = _authService.GenerateToken(userDto);

            return Ok(new LoginResponceDTO
            {
                Token = token,
                User = userDto
            });
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValid = await _authService.VerifyConfirmationCodeAsync(dto.Email, dto.Code);
            if (!isValid)
                return BadRequest("Invalid or expired confirmation code");

            var success = await _authService.ConfirmEmailAsync(dto.Email);
            if (!success)
                return BadRequest("Failed to confirm email");

            var userDto = await _userService.GetByEmailAsync(dto.Email);
            var token = _authService.GenerateToken(userDto);

            return Ok(new LoginResponceDTO
            {
                Token = token,
                User = userDto
            });
        }

        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found");

            if (!user.IsEmailConfirmed)
                return BadRequest("Email not confirmed. Please confirm your email first.");

            var success = await _authService.UpdateProfileAsync(dto);
            if (!success)
                return BadRequest("Failed to update profile");

            return Ok();
        }
    }
}


