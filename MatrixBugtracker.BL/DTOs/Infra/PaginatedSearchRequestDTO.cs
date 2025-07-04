namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginatedSearchRequestDTO : PaginationRequestDTO
    {
        public string SearchQuery { get; init; }
    }
}
