using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.DTOs.Users
{
    public class GetUsersByRoleRequestDTO : PaginationRequestDTO
    {
        public UserRole Role { get; init; }
    }
}
