using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    public class TagsController : BaseController
    {
        [HttpGet]
        [AuthorizeApi]
        public IActionResult Get([FromQuery]bool withArchived)
        {
            return APIResponse(ResponseDTO<object>.NotImplemented());
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public IActionResult Create([FromQuery] string tags)
        {
            return APIResponse(ResponseDTO<object>.NotImplemented());
        }

        [HttpPut]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public IActionResult Archive([FromQuery] string tag)
        {
            return APIResponse(ResponseDTO<object>.NotImplemented());
        }

        [HttpPut]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public IActionResult Unarchive([FromQuery] string tag)
        {
            return APIResponse(ResponseDTO<object>.NotImplemented());
        }
    }
}
