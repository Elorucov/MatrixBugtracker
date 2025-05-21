using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IProductService
    {
        Task<ResponseDTO<int>> CreateAsync(ProductCreateDTO request);
        Task<ResponseDTO<bool>> EditAsync(ProductEditDTO request);
        Task<ResponseDTO<bool>> SetIsOverFlagAsync(int productId, bool flag);
        Task<ResponseDTO<bool>> InviteUserAsync(int productId, int userId);
        Task<ResponseDTO<bool>> KickUserAsync(int productId, int userId);
        Task<ResponseDTO<bool>> JoinAsync(int productId);
        Task<ResponseDTO<bool>> LeaveAsync(int productId);
        Task<PaginationResponseDTO<ProductDTO>> GetAllAsync(PaginationRequestDTO request);
        Task<PaginationResponseDTO<ProductDTO>> GetProductsWithInviteRequestAsync(PaginationRequestDTO request);
        Task<PaginationResponseDTO<ProductDTO>> SearchAsync(PaginatedSearchRequestDTO request);
        ResponseDTO<ProductEnumsDTO> GetEnumValues();
    }
}
