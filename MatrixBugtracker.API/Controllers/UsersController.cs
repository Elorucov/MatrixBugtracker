using MatrixBugtracker.API.Filters;
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
    }
}
