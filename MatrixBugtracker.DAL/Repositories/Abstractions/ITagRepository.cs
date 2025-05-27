using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<List<Tag>> GetIntersectingAsync(string[] tags);
        Task<List<Tag>> GetAllAsync();
        Task<List<Tag>> GetUnarchivedAsync();
        Task AddBatchAsync(string[] tags);
    }
}
