using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/users")]
    [AuthorizeApi]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        // TODO: user stats (how many reports created and reports count by product)
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByIdAsync(int userId)
        {
            return APIResponse(await _service.GetByIdAsync(userId));
        }

        [HttpPut]
        public async Task<IActionResult> EditAsync([FromForm] UserEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        [HttpPatch("change-photo")]
        public async Task<IActionResult> ChangePhotoAsync([FromForm] int photoFileId)
        {
            return APIResponse(await _service.ChangePhotoAsync(photoFileId));
        }
    }
}
