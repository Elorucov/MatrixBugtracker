using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(BugtrackerContext db) : base(db) { }

        public async Task<Report> GetByIdWithIncludesAsync(int id)
        {
            return await _dbSet.Include(r => r.Product)
                .Include(r => r.Tags).ThenInclude(rt => rt.Tag)
                .Include(r => r.Attachments).ThenInclude(ra => ra.File)
                .Include(r => r.Reproduces).ThenInclude(rp => rp.User)
                .SingleOrDefaultAsync(r => r.Id == id);
        }

        // Returns reports for tester users (without vulnerabilities and reports from non-accessible products)
        // if creatorId == 0, filteredProductIds may contains all products that currentUserId joined to
        // if creatorId > 0, filteredProductIds may contains non-open products that currentUserId joined to
        public async Task<PaginationResult<Report>> GetWithRestrictionsAsync(int currentUserId,
            int pageNumber, int pageSize, int creatorId, List<int> filteredProductIds, ReportFilter filter = null)
        {
            if (currentUserId <= 0) throw new ArgumentException($"Value must be greater than 0", nameof(currentUserId));

            var query = _dbSet.AsQueryable();
            if (creatorId > 0)
            {
                query = query.Include(r => r.Product).Where(r => r.CreatorId == creatorId);

                // Getting all products that creatorId is created reports for and non-open
                var nonOpenProductsThatReportsCreated = await query.Include(r => r.Product).GroupBy(r => r.Product)
                    .Select(g => g.Key).Where(p => p.AccessLevel != ProductAccessLevel.Open).ToListAsync();

                // Then get product ids that creatorId is created reports for and currentUser can access to
                var accessibleNonOpenProductIds = nonOpenProductsThatReportsCreated.Where(p => filteredProductIds.Contains(p.Id)).Select(p => p.Id);

                query = query.Where(r => r.Product.AccessLevel == ProductAccessLevel.Open || accessibleNonOpenProductIds.Contains(r.ProductId));
            }
            else
            {
                query = query.Where(r => filteredProductIds.Contains(r.ProductId));
            }

            query = query.WithFilter(filter);
            query = query.Where(r => r.Severity != ReportSeverity.Vulnerability || (r.Severity == ReportSeverity.Vulnerability && r.CreatorId == currentUserId));

            return await query.GetPageAsync(pageNumber, pageSize);
        }

        public async Task<PaginationResult<Report>> GetForProductWithRestrictionAsync(int currentUserId,
            int pageNumber, int pageSize, int productId, int creatorId = 0, ReportFilter filter = null)
        {
            if (currentUserId <= 0) throw new ArgumentException($"Value must be greater than 0", nameof(currentUserId));
            if (productId <= 0) throw new ArgumentException($"Value must be greater than 0", nameof(productId));

            var query = _dbSet.AsQueryable();

            query = query.WithFilter(filter);
            if (creatorId > 0) query = query.Where(r => r.CreatorId == creatorId);
            query = query.Where(r => r.ProductId == productId &&
                (r.Severity != ReportSeverity.Vulnerability || (r.Severity == ReportSeverity.Vulnerability && r.CreatorId == currentUserId)));

            return await query.GetPageAsync(pageNumber, pageSize);
        }

        public async Task<PaginationResult<Report>> GetFilteredAsync(int pageNumber, int pageSize, int productId = 0, int creatorId = 0, ReportFilter filter = null)
        {
            var query = _dbSet.AsQueryable();

            query = query.WithFilter(filter);
            if (creatorId > 0) query = query.Where(r => r.CreatorId == creatorId);
            if (productId > 0) query = query.Where(r => r.ProductId == productId);

            return await query.GetPageAsync(pageNumber, pageSize);
        }

        public async Task AddTagsAsync(Report report, List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                await _db.ReportTags.AddAsync(new ReportTag
                {
                    Report = report,
                    Tag = tag
                });
            }
        }

        public async Task RemoveAllTagsAsync(int reportId)
        {
            await _db.ReportTags.Where(rt => rt.ReportId == reportId).ExecuteDeleteAsync();
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

        public async Task RemoveAllAttachmentsAsync(int reportId)
        {
            await _db.ReportAttachments.Where(rt => rt.ReportId == reportId).ExecuteDeleteAsync();
        }
    }
}
