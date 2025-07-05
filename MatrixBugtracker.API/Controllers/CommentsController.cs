using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for working with reports' comments
    /// </summary>
    [Route("api/v1/comments")]
    [AuthorizeApi]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService _service;
        public CommentsController(ICommentsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get comments for the report
        /// </summary>
        /// <returns>List of comments and mentioned users</returns>
        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> GetAsync([FromQuery] GetCommentsRequestDTO request)
        {
            return APIResponse(await _service.GetAsync(request));
        }

        /// <summary>
        /// Create a new comment for a report
        /// </summary>
        /// <returns>ID of created comment</returns>
        [HttpPost]
        [AuthorizeApi]
        public async Task<IActionResult> CreateAsync(CommentCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }

        /// <summary>
        /// Edit a comment
        /// </summary>
        /// <remarks>
        /// If comment about a report's status or severity update, then such a comment cannot be edited
        /// </remarks>
        /// <returns>'true' if success</returns>
        [HttpPut]
        [AuthorizeApi]
        public async Task<IActionResult> EditAsync(CommentEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpDelete("{commentId}")]
        [AuthorizeApi]
        public async Task<IActionResult> DeleteAsync(int commentId)
        {
            return APIResponse(await _service.DeleteAsync(commentId));
        }
    }
}
