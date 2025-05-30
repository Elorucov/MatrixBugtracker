using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.DAL.Entities;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IFileService
    {
        Task<ResponseDTO<int>> SaveFileAsync(FileUploadDTO request);
        Task<ResponseDTO<UploadedFile>> GetFileEntityAsync(int fileId, bool isImage);
        Task<ResponseDTO<bool>> CheckFilesAccessAsync(int[] fileIds);
        Task<(byte[], string)> GetFileContentByPathAsync(string path);
    }
}
