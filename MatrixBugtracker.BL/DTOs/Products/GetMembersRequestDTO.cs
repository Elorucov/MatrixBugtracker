using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class GetMembersRequestDTO : PaginationRequestDTO
    {
        public int ProductId { get; init; }
    }
}
