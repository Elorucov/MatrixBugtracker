using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Implementations.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BugtrackerContext _context;
        private Dictionary<Type, IRepositoryBase> _repositories;

        public UnitOfWork(BugtrackerContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, IRepositoryBase>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public TRepository GetRepository<TRepository>() where TRepository : class, IRepositoryBase
        {
            Type repoImplType = GetRepositoryImpl<TRepository>();
            if (_repositories.TryGetValue(repoImplType, out IRepositoryBase existing)) return (TRepository)existing;

            IRepositoryBase instance = (IRepositoryBase)Activator.CreateInstance(repoImplType, _context);
            if (instance is TRepository repo)
            {
                _repositories.Add(repoImplType, repo);
                return repo;
            }

            throw new InvalidOperationException($"Could not create repository of type {repoImplType}");
        }

        private Type GetRepositoryImpl<TRepository>()
        {
            string interfaceName = typeof(TRepository).Name;
            string className = interfaceName.StartsWith("I") ? interfaceName.Substring(1) : interfaceName;

            string interfaceNamespace = typeof(TRepository).Namespace;
            string implFullName = $"{interfaceNamespace}.{className}";

            Type repoType = Type.GetType(implFullName);

            if (repoType == null)
            {
                repoType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == className && typeof(TRepository).IsAssignableFrom(t));
            }

            if (repoType == null) throw new InvalidOperationException($"No repository class found for {typeof(TRepository)}");

            return repoType;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
