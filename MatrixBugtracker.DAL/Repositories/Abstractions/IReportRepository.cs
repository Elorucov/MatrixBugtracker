using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IReportRepository : IRepository<Report>
    {
        Task AddTagsAsync(Report report, List<Tag> tags);
        Task AddAttachmentAsync(Report report, List<UploadedFile> files);
    }
}
