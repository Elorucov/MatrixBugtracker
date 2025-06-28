using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(BugtrackerContext db) : base(db) { }

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
