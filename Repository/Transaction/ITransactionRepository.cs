using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TblTransaction>> GetAllAsync();
        Task<TblTransaction> GetByIdAsync(int id);
        Task AddAsync(TblTransaction entity);
        Task UpdateAsync(TblTransaction entity);
        Task DeleteAsync(int id);
    }
}