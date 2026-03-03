using Bed4Head.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bed4Head.BLL.DTO;

namespace Bed4Head_Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto, [FromQuery] string password)
        {
            await _userService.RegisterAsync(userDto, password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            bool isValid = await _userService.VerifyPasswordAsync(email, password);
            if (!isValid) return Unauthorized();
            return Ok();
        }
    }
}
