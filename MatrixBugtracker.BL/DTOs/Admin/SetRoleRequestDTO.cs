using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.BL.DTOs.Admin
{
    public class SetRoleRequestDTO
    {
        public int UserId { get; set; }
        public UserRole Role { get; set; }
    }
}
