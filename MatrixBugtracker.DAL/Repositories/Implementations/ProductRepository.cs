using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BugtrackerContext db) : base(db) { }

        public async Task<bool> HasEntityAsync(string name)
        {
            return await _dbSet.Where(p => p.Name == name).CountAsync() > 0;
        }

        public async Task<ProductMember> GetProductMemberAsync(int productId, int userId)
        {
            return await _db.ProductMembers.SingleOrDefaultAsync(e => e.ProductId == productId && e.MemberId == userId);
        }

        public async Task AddUserToProductAsync(Product product, User user, ProductMemberStatus status)
        {
            await _db.ProductMembers.AddAsync(new ProductMember
            {
                Product = product,
                Member = user,
                Status = status
            });
        }

        public void RemoveUserFromProduct(ProductMember prodMem)
        {
            _db.ProductMembers.Remove(prodMem);
        }

        public void UpdateProductMember(ProductMember prodMem)
        {
            _db.ProductMembers.Update(prodMem);
        }
    }
}
