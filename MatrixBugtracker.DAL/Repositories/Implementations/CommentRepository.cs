using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(BugtrackerContext db) : base(db) { }
    }
}
