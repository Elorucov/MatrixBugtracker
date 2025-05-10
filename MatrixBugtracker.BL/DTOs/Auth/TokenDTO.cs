namespace MatrixBugtracker.BL.DTOs.Auth
{
    public class TokenDTO
    {
        public int UserId { get; set; }
        public string Token { get; init; }
        public DateTime ExpiresAt { get; init; }
    }
}
