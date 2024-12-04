using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<TblAuction>> GetAllAsync();
        Task<TblAuction> GetByIdAsync(int id);
        Task AddAsync(TblAuction entity);
        Task UpdateAsync(TblAuction entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<TblAuction>> GetAuctionsByProductIdAsync(int productId);
    }
}