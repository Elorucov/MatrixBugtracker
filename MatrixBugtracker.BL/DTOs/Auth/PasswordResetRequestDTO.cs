namespace MatrixBugtracker.BL.DTOs.Auth
{
    public class PasswordResetRequestDTO
    {
        public string Email { get; init; }
        public string Code { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
    }
}
