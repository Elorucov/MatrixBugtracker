namespace MatrixBugtracker.BL.DTOs.Auth
{
    public class TokenDTO
    {
        public int UserId { get; set; }
        public string AccessToken { get; init; }
        public DateTime AccessTokenExpiresAt { get; init; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
