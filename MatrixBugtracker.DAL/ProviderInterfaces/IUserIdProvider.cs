namespace MatrixBugtracker.DAL.ProviderInterfaces
{
    // Needs to access an authorized user's id from data layer.
    public interface IUserIdProvider
    {
        int UserId { get; }
    }
}
