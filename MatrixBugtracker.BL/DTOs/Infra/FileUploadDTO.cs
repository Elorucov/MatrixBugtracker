using Microsoft.AspNetCore.Http;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class FileUploadDTO
    {
        public IFormFile File { get; set; }
        public bool IsPhoto { get; set; }
    }
}
