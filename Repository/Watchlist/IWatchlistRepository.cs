using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface IWatchlistRepository
    {
        Task<IEnumerable<TblWatchlist>> GetAllAsync();
        Task<TblWatchlist> GetByIdAsync(int id);
        Task AddAsync(TblWatchlist entity);
        Task UpdateAsync(TblWatchlist entity);
        Task DeleteAsync(int id);
    }
}