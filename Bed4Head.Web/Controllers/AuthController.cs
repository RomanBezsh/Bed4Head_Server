using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);

            if (result == null)
                return BadRequest("User already exists or registration failed");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto) // Заменили Query на Body
        {
            bool isValid = await _authService.VerifyPasswordAsync(dto.Email, dto.Password);
            if (!isValid) return Unauthorized("Invalid email or password");

            var userDto = await _userService.GetByEmailAsync(dto.Email);
            if (userDto == null) return NotFound();

            var token = _authService.GenerateToken(userDto);

            return Ok(new LoginResponceDTO
            {
                Token = token,
                User = userDto
            });
        }
    }
}



