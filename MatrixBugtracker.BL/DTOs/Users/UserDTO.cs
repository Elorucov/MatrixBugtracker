using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.DTOs.Users
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public FileDTO Photo { get; set; }
    }
}
