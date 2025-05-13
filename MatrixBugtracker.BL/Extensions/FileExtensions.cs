using MatrixBugtracker.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace MatrixBugtracker.BL.Extensions
{
    public static class FileExtensions
    {
        static readonly string[] _allowedImageContentTypes = new string[] {
            "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp", "image/heic"
        };

        public static bool IsImage(this UploadedFile file)
        {
            return _allowedImageContentTypes.Contains(file.MimeType);
        }

        public static bool IsImage(this IFormFile file)
        {
            return _allowedImageContentTypes.Contains(file.ContentType);
        }
    }
}
