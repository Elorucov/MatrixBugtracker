using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/tags")]
    public class TagsController : BaseController
    {
        private readonly ITagsService _service;
        public TagsController(ITagsService service)
        {
            _service = service;
        }

        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> Get([FromQuery] bool withArchived)
        {
            return APIResponse(await _service.GetAsync(withArchived));
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> Add([FromQuery] string tagsComma)
        {
            return APIResponse(await _service.AddAsync(tagsComma));
        }

        // TODO: multiple tags (patch with one method)
        [HttpPatch("archive")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> Archive([FromQuery] string tag)
        {
            return APIResponse(await _service.SetArchiveFlag(tag, true));
        }

        // TODO: multiple tags
        [HttpPatch("unarchive")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> UnarchiveAsync([FromQuery] string tag)
        {
            return APIResponse(await _service.SetArchiveFlag(tag, false));
        }
    }
}
