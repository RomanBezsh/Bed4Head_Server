using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Bed4Head.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head_Server.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public AuthController(
            IUserService userService,
            IAuthService authService
            )
        {
            _userService = userService;
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromQuery] UserDTO userDto, [FromQuery] string password)
        {
            await _userService.RegisterAsync(userDto, password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            bool isValid = await _userService.VerifyPasswordAsync(email, password);
            if (!isValid) return Unauthorized("Invalid email or password");

            var userDto = await _userService.GetUserByEmailAsync(email);
            if (userDto == null) return NotFound();

            var token = _authService.GenerateToken(userDto);

            return Ok(new
            {
                Token = token,
                User = userDto
            });
        }
    }
}
