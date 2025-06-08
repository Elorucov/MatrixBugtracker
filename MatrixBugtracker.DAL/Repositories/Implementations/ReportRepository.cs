using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(BugtrackerContext db) : base(db) { }

        public async Task AddTagsAsync(Report report, List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                await _db.ReportTags.AddAsync(new ReportTag { 
                    Report = report,
                    Tag = tag
                });
            }
        }

        public async Task AddAttachmentAsync(Report report, List<UploadedFile> files)
        {
            foreach (var file in files)
            {
                await _db.ReportAttachments.AddAsync(new ReportAttachment
                {
                    Report = report,
                    File = file
                });
            }
        }
    }
}
