using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbBtlLtwncContext _context;

        public ProductRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }

        public async Task<List<TblProduct>> GetProductsByCategory(int categoryId)
        {
            return await _context.TblProducts
                .Where(p => p.ICategoryId == categoryId)
                .Include(p => p.ICategory)
                .Include(p => p.ISeller)
                .ToListAsync();
        }
        public async Task<TblProduct> GetByIdAsync(int productId)
        {
            return await _context.TblProducts
                                 .FirstOrDefaultAsync(p => p.IProductId == productId);
        }

        public async Task AddAsync(TblProduct product)
        {
            await _context.TblProducts.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TblProduct product)
        {
            _context.TblProducts.Update(product);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Xóa các bids liên quan đến auctions của sản phẩm
                    var bidsToRemove = await _context.TblBids
                        .Where(b => _context.TblAuctions
                            .Any(a => a.IProductId == id && a.IAuctionId == b.IAuctionId))
                        .ToListAsync();
                    _context.TblBids.RemoveRange(bidsToRemove);

                    // 2. Xóa các watchlist items liên quan
                    var watchlistItemsToRemove = await _context.TblWatchlists
                        .Where(w => w.IProductId == id)
                        .ToListAsync();
                    _context.TblWatchlists.RemoveRange(watchlistItemsToRemove);

                    // 3. Xóa các auction liên quan
                    var auctionsToRemove = await _context.TblAuctions
                        .Where(a => a.IProductId == id)
                        .ToListAsync();
                    _context.TblAuctions.RemoveRange(auctionsToRemove);

                    // 4. Xóa các review liên quan
                    var reviewsToRemove = await _context.TblReviews
                        .Where(r => r.IProductId == id)
                        .ToListAsync();
                    _context.TblReviews.RemoveRange(reviewsToRemove);

                    // 5. Xóa sản phẩm
                    var product = await _context.TblProducts.FindAsync(id);
                    if (product != null)
                    {
                        _context.TblProducts.Remove(product);
                    }

                    // Lưu các thay đổi
                    await _context.SaveChangesAsync();

                    // Xác nhận transaction
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Hoàn tác nếu có lỗi
                    await transaction.RollbackAsync();
                    throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message, ex);
                }
            }
        }
    }
}