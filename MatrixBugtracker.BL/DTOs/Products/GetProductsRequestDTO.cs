using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.DTOs.Products
{
    public class GetProductsRequestDTO : PaginatedSearchRequestDTO
    {
        public ProductType? Type { get; init; }
    }
}
