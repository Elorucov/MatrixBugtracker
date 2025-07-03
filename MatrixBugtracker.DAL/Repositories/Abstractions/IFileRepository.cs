using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IFileRepository : IRepository<UploadedFile>
    {
        Task<List<UploadedFile>> GetIntersectingAsync(int[] fileIds);
        Task<PaginationResult<UploadedFile>> GetUserFilesAsync(int userId, int number, int size);
    }
}
