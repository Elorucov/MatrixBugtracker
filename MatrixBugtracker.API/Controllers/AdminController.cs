using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs;
using MatrixBugtracker.BL.DTOs.Admin;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/admin")]
    [AuthorizeApi([UserRole.Admin])]
    public class AdminController : BaseController
    {
        private readonly IPlatformService _platformService;
        private readonly IUserService _userService;

        public AdminController(ILogger<AdminController> logger, IPlatformService platformService, IUserService userService)
        {
            _platformService = platformService;
            _userService = userService;
        }

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            return APIResponse(new ResponseDTO<List<EnumValueDTO>>(_platformService.GetRoleEnums()));
        }

        [HttpPost("set-role")]
        public async Task<IActionResult> SetRoleAsync(SetRoleRequestDTO request)
        {
            return APIResponse(await _userService.SetUserRoleAsync(request));
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new ApplicationException("Test error.");
        }

        [HttpPost("test-validation")]
        public IActionResult TestValidation([FromForm] TestRequestDTO request)
        {
            return APIResponse(new ResponseDTO<TestRequestDTO>(request));
        }
    }
}
