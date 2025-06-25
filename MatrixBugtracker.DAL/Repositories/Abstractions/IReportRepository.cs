using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<Report> GetByIdWithIncludesAsync(int id);

        Task<PaginationResult<Report>> GetWithRestrictionsAsync(int currentUserId,
            int pageNumber, int pageSize, int creatorId, List<int> filteredProductIds, ReportFilter filter = null);

        Task<PaginationResult<Report>> GetForProductWithRestrictionAsync(int currentUserId,
            int pageNumber, int pageSize, int productId, int creatorId = 0, ReportFilter filter = null);

        Task<PaginationResult<Report>> GetFilteredAsync(int pageNumber, int pageSize, int productId = 0, int creatorId = 0, ReportFilter filter = null);

        Task AddTagsAsync(Report report, List<Tag> tags);
        Task RemoveAllTagsAsync(int reportId);
        Task AddAttachmentAsync(Report report, List<UploadedFile> files);
        Task RemoveAllAttachmentsAsync(int reportId);
    }
}
