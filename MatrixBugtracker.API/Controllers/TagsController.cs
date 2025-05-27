using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MatrixBugtracker.API.Controllers
{
    public class TagsController : BaseController
    {
        private readonly ITagsService _service;
        public TagsController(ITagsService service)
        {
            _service = service;
        }

        [HttpGet]
        [AuthorizeApi]
        public IActionResult Get([FromQuery]bool withArchived)
        {
            return APIResponse(ResponseDTO<object>.NotImplemented());
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> Add([FromQuery] string tagsComma)
        {
            return APIResponse(await _service.AddAsync(tagsComma));
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
