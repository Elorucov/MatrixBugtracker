using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class FileRepository : Repository<UploadedFile>, IFileRepository
    {
        public FileRepository(BugtrackerContext db) : base(db) { }

        public async Task<List<UploadedFile>> GetIntersectingAsync(int[] fileIds)
        {
            return await _dbSet.Where(f => fileIds.Contains(f.Id)).ToListAsync();
        }
    }
}
