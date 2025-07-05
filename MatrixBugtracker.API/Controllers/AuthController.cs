using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Deals with user authentication and registration
    /// </summary>
    [Route("api/v1/auth")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Logs in the user and verifies email if not already
        /// </summary>
        /// <returns>User ID and tokens</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromForm] LoginRequestDTO request)
        {
            return APIResponse(await _userService.LoginAsync(request));
        }

        /// <summary>
        /// Get new tokens
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <param name="refreshToken">User's previous refresh token</param>
        /// <returns>User ID and tokens</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromForm] int userId, [FromForm] string refreshToken)
        {
            return APIResponse(await _userService.RefreshAsync(userId, refreshToken));
        }

        /// <summary>
        /// Register a new user and send a confirmation code to email for verify
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequestDTO request)
        {
            return APIResponse(await _userService.CreateUserAsync(request));
        }

        /// <summary>
        /// Send a confirmation code for reset user's password to email
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>'true' if success</returns>
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordAsync([FromForm] string email)
        {
            return APIResponse(await _userService.SendPasswordResetConfirmationAsync(email));
        }

        /// <summary>
        /// Set a new password for user
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(PasswordResetRequestDTO request)
        {
            return APIResponse(await _userService.ResetPasswordAsync(request));
        }
    }
}
