using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly DbBtlLtwncContext _context;

        public AuctionRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TblAuction>> GetAllAsync()
        {
            return await _context.TblAuctions
                .Include(a => a.IProduct)
                .Include(a => a.IWinner)
                .ToListAsync();
        }
        public async Task<TblAuction> GetAuctionByProductIdAsync(int productId)
        {
            return await _context.TblAuctions
                .Include(a => a.IProduct)
                    .ThenInclude(p => p.ISeller)
                .Where(a => a.IProductId == productId)
                .OrderByDescending(a => a.DtStartTime)
                .FirstOrDefaultAsync();
        }
        public async Task<List<TblAuction>> GetAuctionsByProductIdAsync(int productId)
        {
            return await _context.TblAuctions
            .Include(a => a.IProduct)
            .Where(a => a.IProductId == productId)
            .ToListAsync();
        }

        public async Task<TblAuction> GetByIdAsync(int id)
        {

            return await _context.TblAuctions
                .Include(a => a.IProduct)
                .Include(a => a.IWinner)
                .FirstOrDefaultAsync(a => a.IAuctionId == id);
        }

        public async Task AddAsync(TblAuction entity)
        {
            await _context.TblAuctions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TblAuction entity)
        {
            _context.TblAuctions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TblAuctions.FindAsync(id);
            if (entity != null)
            {
                _context.TblAuctions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TblTransaction>> GetAuctionTransactions(int auctionId)
        {
            return await _context.TblTransactions
                .Where(t => t.IAuctionId == auctionId)
                .Include(t => t.Buyer) 
                .Include(t => t.Auction)
                .OrderByDescending(t => t.DtTransactionTime)
                .ToListAsync();
        }
    }
}
