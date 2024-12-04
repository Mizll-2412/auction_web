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
        public async Task<IEnumerable<TblAuction>> GetAuctionsByProductIdAsync(int productId)
        {
            return await _context.TblAuctions
                .Where(a => a.IProduct.IProductId == productId)
                .Include(a => a.IProduct)
                .Include(a => a.IWinner)
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
    }
}
