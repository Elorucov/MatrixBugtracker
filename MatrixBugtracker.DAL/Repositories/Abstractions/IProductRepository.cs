using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> HasEntityAsync(string name);
        Task<PaginationResult<Product>> GetPageWithMembersAsync(int number, int size);
        Task<PaginationResult<Product>> GetWithoutSecretProductsAsync(int authorizedUserId, int number, int size);
        Task<ProductMember> GetProductMemberAsync(int productId, int userId);
        Task<PaginationResult<ProductMember>> GetProductsForUserByStatusAsync(ProductMemberStatus status, int userId, int number, int size);
        Task AddUserToProductAsync(Product product, User user, ProductMemberStatus status);
        void RemoveUserFromProduct(ProductMember prodMem);
        void UpdateProductMember(ProductMember prodMem);
    }
}
