using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(BugtrackerContext db) : base(db) { }

        public async Task<List<Tag>> GetIntersectingAsync(string[] tags)
        {
            return await _dbSet.Where(t => tags.Contains(t.Name)).ToListAsync();
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<Tag>> GetUnarchivedAsync()
        {
            return await _dbSet.Where(t => !t.IsArchived).ToListAsync();
        }

        public async Task AddBatchAsync(string[] tags)
        {
            var newTags = tags.Select(t => new Tag { Name = t });
            await _dbSet.AddRangeAsync(newTags);
        }
    }
}
