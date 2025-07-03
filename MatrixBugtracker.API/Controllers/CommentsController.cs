using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/comments")]
    [AuthorizeApi]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService _service;
        public CommentsController(ICommentsService service)
        {
            _service = service;
        }

        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> GetAsync([FromQuery] GetCommentsRequestDTO request)
        {
            return APIResponse(await _service.GetAsync(request));
        }

        [HttpPost]
        [AuthorizeApi]
        public async Task<IActionResult> CreateAsync(CommentCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }

        [HttpPut]
        [AuthorizeApi]
        public async Task<IActionResult> EditAsync(CommentEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        [HttpDelete("{commentId}")]
        [AuthorizeApi]
        public async Task<IActionResult> DeleteAsync(int commentId)
        {
            return APIResponse(await _service.DeleteAsync(commentId));
        }
    }
}
