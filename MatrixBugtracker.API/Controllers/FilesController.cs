using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/files")]
    [AuthorizeApi]
    public class FilesController : BaseController
    {
        private readonly IFileService _service;

        public FilesController(IFileService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileUploadDTO request)
        {
            return APIResponse(await _service.SaveFileAsync(request));
        }

        [HttpGet("{path}")]
        public async Task<IResult> Download(string path)
        {
            var (content, type) = await _service.GetFileContentByPathAsync(path);
            return Results.File(content, type);
        }
    }
}
