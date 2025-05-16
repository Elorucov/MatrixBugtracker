using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> HasEntityAsync(string name);
        Task<ProductMember> GetProductMemberAsync(int productId, int userId);
        Task AddUserToProductAsync(Product product, User user, ProductMemberStatus status);
        void RemoveUserFromProduct(ProductMember prodMem);
        void UpdateProductMember(ProductMember prodMem);
    }
}
