namespace MatrixBugtracker.BL.DTOs.Auth
{
    public class RegisterRequestDTO
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
    }
}
