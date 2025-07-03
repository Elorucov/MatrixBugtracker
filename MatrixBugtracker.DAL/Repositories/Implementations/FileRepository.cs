using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
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

        public async Task<PaginationResult<UploadedFile>> GetUserFilesAsync(int userId, int number, int size)
        {
            return await _dbSet.Where(f => f.CreatorId == userId).GetPageAsync(number, size);
        }
    }
}
