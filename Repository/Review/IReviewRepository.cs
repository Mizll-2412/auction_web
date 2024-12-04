using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<TblReview>> GetAllAsync();
        Task<TblReview> GetByIdAsync(int id);
        Task AddAsync(TblReview entity);
        Task UpdateAsync(TblReview entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<TblReview>> GetReviewsByProductIdAsync(int productId);
    }
}