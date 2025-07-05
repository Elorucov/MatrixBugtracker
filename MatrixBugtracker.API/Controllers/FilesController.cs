using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for working with files
    /// </summary>
    [Route("api/v1/files")]
    public class FilesController : BaseController
    {
        private readonly IFileService _service;

        public FilesController(IFileService service)
        {
            _service = service;
        }

        /// <summary>
        /// Uploads a file to the server
        /// </summary>
        /// <returns>ID of the uploaded file</returns>
        [HttpPost]
        [AuthorizeApi]
        public async Task<IActionResult> UploadAsync([FromForm] FileUploadDTO request)
        {
            return APIResponse(await _service.SaveFileAsync(request));
        }

        /// <summary>
        /// Get authenticated user's uploaded files
        /// </summary>
        /// <returns>List of uploaded files</returns>
        [HttpGet("my")]
        [AuthorizeApi]
        public async Task<IActionResult> GetCurrentUserFilesAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetCurrentUserFilesAsync(request));
        }

        /// <summary>
        /// Get all uploaded files
        /// </summary>
        /// <returns>List of uploaded files</returns>
        [HttpGet("all")]
        [AuthorizeApi([UserRole.Admin])]
        public async Task<IActionResult> GetAllFilesAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetAllFilesAsync(request));
        }

        /// <summary>
        /// Download a file
        /// </summary>
        [HttpGet("{path}")]
        [AuthorizeApi]
        public async Task<IResult> DownloadAsync(string path)
        {
            var (content, type) = await _service.GetFileContentByPathAsync(path);
            return Results.File(content, type);
        }
    }
}
