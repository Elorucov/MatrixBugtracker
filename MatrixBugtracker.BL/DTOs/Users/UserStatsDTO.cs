namespace MatrixBugtracker.BL.DTOs.Users
{
    public class UserStatsDTO
    {
        public int TotalReports { get; init; }
        public int AcceptedReports { get; init; }
        public int JoinedProducts { get; init; }
        public List<UserStatProductDTO> TopProducts { get; init; }
    }
}
