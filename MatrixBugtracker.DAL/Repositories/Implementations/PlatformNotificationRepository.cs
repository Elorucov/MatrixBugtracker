using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class PlatformNotificationRepository : Repository<PlatformNotification>, IPlatformNotificationRepository
    {
        public PlatformNotificationRepository(BugtrackerContext db) : base(db) { }
    }
}
