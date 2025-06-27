using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
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
        public async Task<IActionResult> GetAsync([FromQuery] bool withArchived)
        {
            return APIResponse(await _service.GetAsync(withArchived));
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> AddAsync([FromBody] string[] tags)
        {
            return APIResponse(await _service.AddAsync(tags));
        }

        // TODO: multiple tags?
        [HttpPatch("archive")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> ArchiveAsync([FromForm] string tag)
        {
            return APIResponse(await _service.SetArchiveFlag(tag, true));
        }

        // TODO: multiple tags? (and merge two methods into one)
        [HttpPatch("unarchive")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> UnarchiveAsync([FromForm] string tag)
        {
            return APIResponse(await _service.SetArchiveFlag(tag, false));
        }
    }
}
