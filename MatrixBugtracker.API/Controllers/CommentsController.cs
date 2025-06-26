using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/comments")]
    [AuthorizeApi]
    public class CommentsController : BaseController
    {
        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> GetAsync([FromQuery] GetCommentsRequestDTO request)
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<int?>.NotImplemented());
        }

        [HttpPost]
        [AuthorizeApi]
        public async Task<IActionResult> CreateAsync([FromForm] CommentCreateDTO request)
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<int?>.NotImplemented());
        }

        [HttpPut]
        [AuthorizeApi]
        public async Task<IActionResult> EditAsync([FromForm] CommentCreateDTO request)
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<int?>.NotImplemented());
        }

        [HttpDelete]
        [AuthorizeApi]
        public async Task<IActionResult> DeleteAsync([FromQuery] int commentId)
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<int?>.NotImplemented());
        }
    }
}
