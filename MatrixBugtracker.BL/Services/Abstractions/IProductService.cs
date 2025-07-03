using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IProductService
    {
        Task<ResponseDTO<int?>> CreateAsync(ProductCreateDTO request);
        Task<ResponseDTO<bool>> EditAsync(ProductEditDTO request);
        Task<ResponseDTO<bool>> ChangeIsOverFlagAsync(int productId, bool isOver);
        Task<ResponseDTO<bool>> InviteUserAsync(ProductUserRequestDTO request);
        Task<ResponseDTO<bool>> KickUserAsync(ProductUserRequestDTO request);
        Task<ResponseDTO<bool>> JoinAsync(int productId);
        Task<ResponseDTO<bool>> LeaveAsync(int productId);
        Task<PaginationResponseDTO<ProductDTO>> GetAllAsync(GetProductsRequestDTO request);
        Task<ResponseDTO<ProductDTO>> GetByIdAsync(int productId);
        Task<PaginationResponseDTO<ProductDTO>> GetProductsByUserMembershipAsync(int userId, ProductMemberStatus status, PaginationRequestDTO request);
        Task<PaginationResponseDTO<ProductDTO>> GetProductsWithInviteRequestAsync(PaginationRequestDTO request);
        Task<PaginationResponseDTO<ProductDTO>> GetJoinedProductsAsync(PaginationRequestDTO request);
        Task<PaginationResponseDTO<UserDTO>> GetJoinRequestUsers(GetJoinRequestUsersReqDTO request);
        Task<ResponseDTO<Product>> CheckAccessAsync(int productId, bool toCreateReport, int userId = 0);
    }
}
