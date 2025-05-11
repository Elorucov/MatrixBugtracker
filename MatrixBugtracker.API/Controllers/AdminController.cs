using Azure.Core;
using MatrixBugtracker.BL.DTOs;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IPlatformService _platformService;

        public AdminController(ILogger<AdminController> logger, IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            return APIResponse(new ResponseDTO<List<EnumValueDTO>>(_platformService.GetRoleEnums()));
        }

        [HttpGet]
        public IActionResult TestError()
        {
            throw new ApplicationException("Test error.");
        }

        [HttpPost]
        public IActionResult TestValidation([FromForm] TestRequestDTO request)
        {
            return APIResponse(new ResponseDTO<TestRequestDTO>(request));
        }
    }
}
