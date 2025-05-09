namespace MatrixBugtracker.Abstractions
{
    // Needs to access an authorized user's id from data layer.
    public interface IUserIdProvider
    {
        int UserId { get; }
    }
}
