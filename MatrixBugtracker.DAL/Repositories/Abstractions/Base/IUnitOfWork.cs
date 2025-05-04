namespace MatrixBugtracker.DAL.Repositories.Abstractions.Base
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository GetRepository<TRepository>() where TRepository : class, IRepositoryBase;
        Task<int> CommitAsync();
    }
}
