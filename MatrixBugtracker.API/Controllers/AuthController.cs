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
        public async Task<IActionResult> LoginAsync(LoginRequestDTO request)
        {
            return APIResponse(await _userService.LoginAsync(request));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromForm] int userId, [FromForm] string refreshToken)
        {
            return APIResponse(await _userService.RefreshAsync(userId, refreshToken));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequestDTO request)
        {
            return APIResponse(await _userService.CreateUserAsync(request));
        }

        [HttpPost("reset-password-request")]
        public async Task<IActionResult> RequestPasswordAsync([FromForm] string email)
        {
            return APIResponse(await _userService.SendPasswordResetConfirmationAsync(email));
        }

        [HttpPost("reset-password-confirm")]
        public async Task<IActionResult> ResetPasswordAsync(PasswordResetRequestDTO request)
        {
            return APIResponse(await _userService.ResetPasswordAsync(request));
        }
    }
}
