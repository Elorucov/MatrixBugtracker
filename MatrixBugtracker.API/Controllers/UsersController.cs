using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for working with tags
    /// </summary>
    [Route("api/v1/users")]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns an info about the user
        /// </summary>
        /// <returns>User with extended info</returns>
        [HttpGet("{userId}")]
        [AuthorizeApi]
        public async Task<IActionResult> GetByIdAsync(int userId)
        {
            return APIResponse(await _service.GetByIdAsync(userId));
        }

        /// <summary>
        /// Returns users by role
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("get-by-role")]
        [AuthorizeApi([UserRole.Admin])]
        public async Task<IActionResult> GetByRoleAsync([FromQuery] GetUsersByRoleRequestDTO request)
        {
            return APIResponse(await _service.GetUsersByRoleAsync(request));
        }

        /// <summary>
        /// Find users by first and last names
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("search")]
        [AuthorizeApi]
        public async Task<IActionResult> SearchAsync([FromQuery] PaginatedSearchRequestDTO request)
        {
            return APIResponse(await _service.SearchUsersAsync(request));
        }

        /// <summary>
        /// Edits authenticated user's info
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPut]
        [AuthorizeApi]
        public async Task<IActionResult> EditAsync(UserEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        /// <summary>
        /// Change authenticated user's profile photo (avatar)
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("avatar")]
        [AuthorizeApi]
        public async Task<IActionResult> ChangeAvatarAsync([FromForm] int photoFileId)
        {
            return APIResponse(await _service.ChangePhotoAsync(photoFileId));
        }

        /// <summary>
        /// Delete authenticated user's profile photo (avatar)
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpDelete("avatar")]
        [AuthorizeApi]
        public async Task<IActionResult> DeleteAvatarAsync()
        {
            return APIResponse(await _service.DeletePhotoAsync());
        }
    }
}
