using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(BugtrackerContext db) : base(db) { }

        public async Task<PaginationResult<Comment>> GetForReportAsync(int reportId, int number, int size)
        {
            return await _dbSet.Include(c => c.Creator)
                .Where(c => c.ReportId == reportId).GetPageAsync(number, size);
        }

        public async Task RemoveAllAttachmentsAsync(int commentId)
        {
            await _db.CommentAttachments.Where(rt => rt.CommentId == commentId).ExecuteDeleteAsync();
        }

        public async Task AddAttachmentAsync(Comment comment, List<UploadedFile> files)
        {
            foreach (var file in files)
            {
                await _db.CommentAttachments.AddAsync(new CommentAttachment
                {
                    Comment = comment,
                    File = file
                });
            }
        }
    }
}
