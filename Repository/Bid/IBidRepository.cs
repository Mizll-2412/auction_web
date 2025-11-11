using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface IBidRepository
    {
        Task<IEnumerable<TblBid>> GetAllAsync();
        Task<TblBid> GetByIdAsync(int id);
        Task<List<TblBid>> GetBidsByAuctionIdAsync(int auctionId);

        Task AddAsync(TblBid entity);
        Task UpdateAsync(TblBid entity);
        Task DeleteAsync(int id);
    }
}