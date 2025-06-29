using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<PaginationResult<Comment>> GetForReportAsync(int reportId, int number, int size);
        Task RemoveAllAttachmentsAsync(int commentId);
        Task AddAttachmentAsync(Comment comment, List<UploadedFile> files);
    }
}
