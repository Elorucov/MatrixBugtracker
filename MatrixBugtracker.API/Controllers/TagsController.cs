using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for working with tags
    /// </summary>
    [Route("api/v1/tags")]
    public class TagsController : BaseController
    {
        private readonly ITagsService _service;
        public TagsController(ITagsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns tags
        /// </summary>
        /// <param name="withArchived">Returns archived tags too</param>
        /// <returns>List of tags</returns>
        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> GetAsync([FromQuery] bool withArchived)
        {
            return APIResponse(await _service.GetAsync(withArchived));
        }

        /// <summary>
        /// Add tags into DB
        /// </summary>
        /// <returns>Count of the successfully added tags and list of already existing tags</returns>
        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> AddAsync([FromBody] string[] tags)
        {
            return APIResponse(await _service.AddAsync(tags));
        }

        /// <summary>
        /// Archives the tag so it can't be used
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("archive")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> ArchiveAsync([FromForm] string tag)
        {
            return APIResponse(await _service.SetArchiveFlag(tag, true));
        }

        /// <summary>
        /// Unarchives the tag
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("unarchive")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> UnarchiveAsync([FromForm] string tag)
        {
            return APIResponse(await _service.SetArchiveFlag(tag, false));
        }
    }
}
