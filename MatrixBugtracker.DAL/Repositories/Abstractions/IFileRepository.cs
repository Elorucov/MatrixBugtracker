using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IFileRepository : IRepository<UploadedFile>
    {
        Task<List<UploadedFile>> GetIntersectingAsync(int[] fileIds);
    }
}
