namespace MatrixBugtracker.Abstractions
{
    // Needs to hash password in DAL and BL
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
