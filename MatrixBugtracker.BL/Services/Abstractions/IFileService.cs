using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IFileService
    {
        Task<ResponseDTO<int>> SaveFileAsync(FileUploadDTO request);
        Task<ResponseDTO<UploadedFile>> GetFileEntityAsync(int fileId, bool isImage);
        Task<ResponseDTO<PageDTO<FileDTO>>> GetCurrentUserFilesAsync(PaginationRequestDTO request);
        Task<ResponseDTO<PageDTO<FileAdminDTO>>> GetAllFilesAsync(PaginationRequestDTO request);
        Task<ResponseDTO<List<UploadedFile>>> CheckFilesAccessAsync(int[] fileIds);
        Task<(byte[], string)> GetFileContentByPathAsync(string path);
    }
}
