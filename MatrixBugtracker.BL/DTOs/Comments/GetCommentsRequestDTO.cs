using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.DTOs.Comments
{
    public class GetCommentsRequestDTO : PaginationRequestDTO
    {
        public int ReportId { get; init; }
    }
}
