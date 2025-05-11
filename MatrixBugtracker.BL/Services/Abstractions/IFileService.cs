using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IFileService
    {
        Task<ResponseDTO<int>> SaveFileAsync(FileUploadDTO request);
        Task<(byte[], string)> GetFileContentByPathAsync(string path);
    }
}
