using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> HasEntityAsync(string name);
        Task<PaginationResult<Product>> GetPageWithMembersAsync(int number, int size, ProductType? type, string searchQuery = null);
        Task<List<Product>> GetByIdsWithMembersAsync(IEnumerable<int> ids);
        Task<PaginationResult<Product>> GetWithoutSecretProductsAsync(int authorizedUserId, int number, int size, ProductType? type, string searchQuery = null);
        Task<Product> GetByIdWithMembersAsync(int productId);
        Task<ProductMember> GetProductMemberAsync(int productId, int userId);
        Task<int> GetMembersCountAsync(int productId);
        Task<int> GetUserJoinedProductsCountAsync(int userId);
        Task<PaginationResult<Product>> GetProductsForUserByStatusAsync(ProductMemberStatus status, int userId, int number, int size);
        Task<PaginationResult<User>> GetUsersForProductByStatusAsync(ProductMemberStatus status, int productId, int number, int size);
        Task AddUserToProductAsync(int productId, int userId, ProductMemberStatus status);
        void RemoveUserFromProduct(ProductMember prodMem);
        void UpdateProductMember(ProductMember prodMem);
    }
}
