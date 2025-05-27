using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class GetJoinRequestUsersReqDTO : PaginationRequestDTO
    {
        public int ProductId { get; set; }
    }
}
