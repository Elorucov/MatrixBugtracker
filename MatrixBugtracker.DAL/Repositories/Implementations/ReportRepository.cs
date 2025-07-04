using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

            // These includes required for getting report creator names and product names
            var query = _dbSet.Include(r => r.Product).ThenInclude(p => p.PhotoFile)
                .Include(r => r.Creator).ThenInclude(u => u.PhotoFile).AsQueryable();
            if (creatorId > 0)
            {
                query = query.Where(r => r.CreatorId == creatorId);

                // Getting all products that creatorId is created reports for and non-open
                var nonOpenProductsThatReportsCreated = await query.GroupBy(r => r.Product)
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

            var query = _dbSet.Include(r => r.Product).ThenInclude(p => p.PhotoFile)
                .Include(r => r.Creator).ThenInclude(u => u.PhotoFile).AsQueryable();

            query = query.WithFilter(filter);
            if (creatorId > 0) query = query.Where(r => r.CreatorId == creatorId);
            query = query.Where(r => r.ProductId == productId &&
                (r.Severity != ReportSeverity.Vulnerability || (r.Severity == ReportSeverity.Vulnerability && r.CreatorId == currentUserId)));

            return await query.GetPageAsync(pageNumber, pageSize);
        }

        public async Task<PaginationResult<Report>> GetFilteredAsync(int pageNumber, int pageSize, int productId = 0, int creatorId = 0, ReportFilter filter = null)
        {
            var query = _dbSet.Include(r => r.Product).ThenInclude(p => p.PhotoFile)
                .Include(r => r.Creator).ThenInclude(u => u.PhotoFile).AsQueryable();

            query = query.WithFilter(filter);
            if (creatorId > 0) query = query.Where(r => r.CreatorId == creatorId);
            if (productId > 0) query = query.Where(r => r.ProductId == productId);

            return await query.GetPageAsync(pageNumber, pageSize);
        }

        public async Task<Dictionary<byte, int>> GetStatusCountersAsync(Expression<Func<Report, bool>> condition)
        {
            var query = _dbSet.Where(condition);
            var totalCount = await query.CountAsync();

            var statusCounters = await query.GroupBy(r => r.Status).Select(r => new
            {
                Status = r.Key,
                Count = r.Count()
            }).ToListAsync();

            Dictionary<byte, int> result = new Dictionary<byte, int>();
            result.Add(byte.MaxValue, totalCount);
            foreach (var group in statusCounters) result.Add((byte)group.Status, group.Count);

            return result;
        }

        public async Task<Dictionary<byte, int>> GetStatusCountersByProductAsync(int productId)
        {
            return await GetStatusCountersAsync(r => r.ProductId == productId);
        }

        public async Task<Dictionary<byte, int>> GetStatusCountersByUserAsync(int userId)
        {
            return await GetStatusCountersAsync(r => r.CreatorId == userId);
        }

        public async Task<Dictionary<int, int>> GetUserReportsCountGroupedByProductAsync(int userId)
        {
            // ThenInclude(p => p.ProductMembers) required for ProductDTO.MembershipStatus property.
            var query = _dbSet.Include(r => r.Product).ThenInclude(p => p.ProductMembers).Where(r => r.CreatorId == userId);
            var totalCount = await query.CountAsync();

            // Getting report counts only for 3 open products
            var statusCounters = await query.GroupBy(r => r.Product)
                .Where(r => r.Key.AccessLevel == ProductAccessLevel.Open).Select(r => new
                {
                    ProductId = r.Key.Id,
                    Count = r.Count()
                }).OrderByDescending(g => g.Count).Take(3).ToListAsync();

            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (var group in statusCounters) result.Add(group.ProductId, group.Count);

            return result;
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

        public async Task<List<User>> GetReproducedUsersAsync(int reportId)
        {
            return await _db.ReportReproduces.Include(rr => rr.User).Select(rr => rr.User).ToListAsync();
        }

        public async Task<ReportReproduce> GetReproducedUserAsync(int reportId, int userId)
        {
            return await _db.ReportReproduces.SingleOrDefaultAsync(rr => rr.ReportId == reportId && rr.UserId == userId);
        }

        public async Task AddReproducedUserAsync(int reportId, int userId)
        {
            await _db.ReportReproduces.AddAsync(new ReportReproduce
            {
                ReportId = reportId,
                UserId = userId
            });
        }

        public void DeleteReproducedUser(ReportReproduce entity)
        {
            _db.ReportReproduces.Remove(entity);
        }
    }
}
