using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BugtrackerContext db) : base(db) { }

        public async Task<bool> HasEntityAsync(string name)
        {
            return await _dbSet.Where(p => p.Name == name).CountAsync() > 0;
        }

        public async Task<PaginationResult<Product>> GetPageWithMembersAsync(int number, int size)
        {
            return await _dbSet.Include(pm => pm.ProductMembers)
                .GetPageAsync(number, size);
        }

        public async Task<PaginationResult<Product>> SearchAsync(string query, int number, int size)
        {
            return await _dbSet.Include(pm => pm.ProductMembers)
                .Where(p => p.Name.ToLower().Contains(query.ToLower()))
                .GetPageAsync(number, size);
        }

        public async Task<PaginationResult<Product>> GetWithoutSecretProductsAsync(int authorizedUserId, int number, int size)
        {
            ProductMemberStatus[] statuses = { ProductMemberStatus.InviteReceived, ProductMemberStatus.Joined };
            var joinedSecretProducts = await _db.ProductMembers.Include(p => p.Product)
                .Where(p => p.MemberId == authorizedUserId && p.Product.AccessLevel == ProductAccessLevel.Secret && statuses.Contains(p.Status))
                .Select(p => p.Product)
                .ToListAsync();

            return await _dbSet.Include(pm => pm.ProductMembers)
                .Where(p => p.AccessLevel != ProductAccessLevel.Secret || joinedSecretProducts.Contains(p))
                .GetPageAsync(number, size);
        }

        public async Task<PaginationResult<Product>> SearchWithoutSecretProductsAsync(int authorizedUserId, string query, int number, int size)
        {
            ProductMemberStatus[] statuses = { ProductMemberStatus.InviteReceived, ProductMemberStatus.Joined };
            var joinedSecretProducts = await _db.ProductMembers.Include(p => p.Product)
                .Where(p => p.MemberId == authorizedUserId && p.Product.AccessLevel == ProductAccessLevel.Secret && statuses.Contains(p.Status))
                .Select(p => p.Product)
                .ToListAsync();

            return await _dbSet.Include(pm => pm.ProductMembers)
                .Where(p => p.Name.ToLower().Contains(query.ToLower()) && (p.AccessLevel != ProductAccessLevel.Secret || joinedSecretProducts.Contains(p)))
                .GetPageAsync(number, size);
        }

        public async Task<ProductMember> GetProductMemberAsync(int productId, int userId)
        {
            return await _db.ProductMembers.SingleOrDefaultAsync(e => e.ProductId == productId && e.MemberId == userId);
        }

        public async Task<PaginationResult<ProductMember>> GetProductsForUserByStatusAsync(ProductMemberStatus status, int userId, int number, int size)
        {
            return await _db.ProductMembers.Include(pm => pm.Product)
                .Where(e => e.Status == status && e.MemberId == userId)
                .GetPageAsync(number, size);
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
