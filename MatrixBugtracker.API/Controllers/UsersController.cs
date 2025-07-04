using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/users")]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]
        [AuthorizeApi]
        public async Task<IActionResult> GetByIdAsync(int userId)
        {
            return APIResponse(await _service.GetByIdAsync(userId));
        }

        [HttpGet("get-by-role")]
        [AuthorizeApi([UserRole.Admin])]
        public async Task<IActionResult> GetByRoleAsync([FromQuery] GetUsersByRoleRequestDTO request)
        {
            return APIResponse(await _service.GetUsersByRoleAsync(request));
        }

        [HttpGet("search")]
        [AuthorizeApi]
        public async Task<IActionResult> SearchAsync([FromQuery] PaginatedSearchRequestDTO request)
        {
            return APIResponse(await _service.SearchUsersAsync(request));
        }

        [HttpPut]
        [AuthorizeApi]
        public async Task<IActionResult> EditAsync(UserEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        [HttpPatch("change-photo")]
        [AuthorizeApi]
        public async Task<IActionResult> ChangePhotoAsync([FromForm] int photoFileId)
        {
            return APIResponse(await _service.ChangePhotoAsync(photoFileId));
        }
    }
}
