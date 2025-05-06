using MatrixBugtracker.BL.DTOs;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;

        public AdminController(ILogger<AdminController> logger, IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Init()
        {
            return APIResponse(await _userService.CreateFirstUserAsync());
        }

        [HttpGet]
        public IActionResult TestError()
        {
            throw new ApplicationException("This is a crash. Not bandicoot, but a crash.");
        }

        [HttpPost]
        public IActionResult TestValidation([FromForm] TestRequestDTO request)
        {
            return APIResponse(new BL.DTOs.Infra.ResponseDTO<TestRequestDTO>(request));
        }
    }
}
