using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task RemoveAllAttachmentsAsync(int commentId);
        Task AddAttachmentAsync(Comment comment, List<UploadedFile> files);
    }
}
