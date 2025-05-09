namespace MatrixBugtracker.BL.DTOs.Auth
{
    public class TokenDTO
    {
        public string Token { get; init; }
        public DateTime ExpiresAt { get; init; }
    }
}
