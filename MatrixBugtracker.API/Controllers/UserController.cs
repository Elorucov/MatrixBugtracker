using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm]LoginRequestDTO request)
        {
            return APIResponse(await _userService.LoginAsync(request));
        }
    }
}
