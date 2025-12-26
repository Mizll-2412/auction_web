using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface IProductRepository
    {
        Task<List<TblProduct>> GetProductsByCategory(int categoryId);
        Task<TblProduct> GetByIdAsync(int productId);
        Task AddAsync(TblProduct product);
        Task UpdateAsync(TblProduct product);
        Task DeleteAsync(int id);
        Task<List<TblProduct>> GetProductsByUserAndCategory(int userId, int categoryId);

    }
}