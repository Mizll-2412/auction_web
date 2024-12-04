using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly DbBtlLtwncContext _context;

        public BidRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TblBid>> GetAllAsync()
        {
            return await _context.TblBids
                .Include(p => p.IAuction)
                .Include(p => p.IBidder)
                .ToListAsync();
        }

        public async Task<TblBid> GetByIdAsync(int id)
        {
            return await _context.TblBids.FindAsync(id);
        }

        public async Task AddAsync(TblBid entity)
        {
            await _context.TblBids.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TblBid entity)
        {
            _context.TblBids.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TblBids.FindAsync(id);
            if (entity != null)
            {
                _context.TblBids.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
