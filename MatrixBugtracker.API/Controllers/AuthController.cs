using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/auth")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequestDTO request)
        {
            return APIResponse(await _userService.LoginAsync(request));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromForm] int userId, [FromForm] string refreshToken)
        {
            return APIResponse(await _userService.RefreshAsync(userId, refreshToken));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDTO request)
        {
            return APIResponse(await _userService.CreateUserAsync(request));
        }
    }
}
