using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Extensions
{
    internal static class QueryExtensions
    {
        public static async Task<PaginationResult<T>> GetPageAsync<T>(this IQueryable<T> query, int number, int size)
        {
            int offset = size * (number - 1);
            int count = await query.CountAsync();
            var items = await query.Skip(offset).Take(size).ToListAsync();

            return new PaginationResult<T>(items, count);
        }

        public static IQueryable<Report> WithFilter(this IQueryable<Report> query, ReportFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.SearchQuery)) query = query.Where(r => r.Title.ToLower().Contains(filter.SearchQuery.ToLower()));
            if (filter?.Severities?.Count > 0) query = query.Where(r => filter.Severities.Contains(r.Severity));
            if (filter?.ProblemTypes?.Count > 0) query = query.Where(r => filter.ProblemTypes.Contains(r.ProblemType));
            if (filter?.Statuses?.Count > 0) query = query.Where(r => filter.Statuses.Contains(r.Status));

            if (filter?.Tags?.Count > 0)
            {
                var tagIds = filter.Tags.Select(t => t.Id);
                query = query.Include(r => r.Tags);
                query = query.Where(r => r.Tags.Any(rt => tagIds.Contains(rt.TagId)));
            }

            // By default (Reverse = false), reports returns in order "newest to oldest"
            if (!filter.Reverse) query = query.OrderByDescending(r => r.Id);

            return query;
        }
    }
}
