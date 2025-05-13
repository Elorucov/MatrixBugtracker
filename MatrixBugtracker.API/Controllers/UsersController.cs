using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [AuthorizeApi]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int userId)
        {
            return APIResponse(await _service.GetByIdAsync(userId));
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm]UserEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        [HttpPut]
        public async Task<IActionResult> ChangePhoto([FromForm] int photoFileId)
        {
            return APIResponse(await _service.ChangePhotoAsync(photoFileId));
        }
    }
}
