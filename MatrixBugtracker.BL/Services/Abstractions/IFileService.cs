using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.DAL.Entities;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IFileService
    {
        Task<ResponseDTO<int>> SaveFileAsync(FileUploadDTO request);
        Task<ResponseDTO<UploadedFile>> GetFileEntityAsync(int fileId, bool isImage);
        Task<(byte[], string)> GetFileContentByPathAsync(string path);
    }
}
