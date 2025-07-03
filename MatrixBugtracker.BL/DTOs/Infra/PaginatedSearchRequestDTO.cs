namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginatedSearchRequestDTO : PaginationRequestDTO
    {
        public string Query { get; init; }
    }
}
