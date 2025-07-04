namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginationRequestDTO
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public static readonly PaginationRequestDTO Infinity = new PaginationRequestDTO { PageNumber = 1, PageSize = int.MaxValue };
    }
}
