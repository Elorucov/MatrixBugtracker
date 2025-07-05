using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Admin;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for admins
    /// </summary>
    [Route("api/v1/admin")]
    [AuthorizeApi([UserRole.Admin])]
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;

        public AdminController(ILogger<AdminController> logger, IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Change user's role
        /// </summary>
        /// <returns>'true' if success</returns>

        [HttpPost("set-role")]
        public async Task<IActionResult> SetRoleAsync(SetRoleRequestDTO request)
        {
            return APIResponse(await _userService.SetUserRoleAsync(request));
        }
    }
}
