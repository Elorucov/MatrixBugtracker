using MatrixBugtracker.DAL.Models;
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
    }
}
