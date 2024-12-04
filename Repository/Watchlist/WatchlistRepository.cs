using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly DbBtlLtwncContext _context;

        public WatchlistRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TblWatchlist>> GetAllAsync()
        {
            return await _context.TblWatchlists
                .Include(w => w.IProduct) // Include the navigation property for product
                .ThenInclude(p => p.ISeller)
                .Include(w => w.IUser) // Include the user navigation property
                .ToListAsync();
        }

        public async Task<TblWatchlist> GetByIdAsync(int id)
        {
            return await _context.TblWatchlists
                .Include(w => w.IProduct)  // Bao gồm thông tin sản phẩm
                .ThenInclude(p => p.ISeller) // Bao gồm thông tin người bán của sản phẩm
                .Include(w => w.IProduct.TblAuctions)  // Bao gồm thông tin đấu giá của sản phẩm
                .FirstOrDefaultAsync(w => w.IWatchlistId == id);
        }

        public async Task AddAsync(TblWatchlist entity)
        {
            await _context.TblWatchlists.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TblWatchlist entity)
        {
            _context.TblWatchlists.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TblWatchlists.FindAsync(id);
            if (entity != null)
            {
                _context.TblWatchlists.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
