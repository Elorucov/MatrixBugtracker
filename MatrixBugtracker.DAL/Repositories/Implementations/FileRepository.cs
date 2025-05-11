using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class FileRepository : Repository<UploadedFile>, IFileRepository
    {
        public FileRepository(BugtrackerContext db) : base(db) { }
    }
}
