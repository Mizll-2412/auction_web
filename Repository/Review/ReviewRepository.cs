using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DbBtlLtwncContext _context;

        public ReviewRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TblReview>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.TblReviews
                .Where(r => r.IProductId == productId)
                .Include(r => r.IProduct)
                .Include(r => r.IReviewer)
                .ToListAsync();
        }

        public async Task<IEnumerable<TblReview>> GetAllAsync()
        {
            return await _context.TblReviews
                .Include(p => p.IProduct)
                .Include(p => p.IReviewer)
                .ToListAsync();
        }

        public async Task<TblReview> GetByIdAsync(int id)
        {
            return await _context.TblReviews.FindAsync(id);
        }

        public async Task AddAsync(TblReview entity)
        {
            await _context.TblReviews.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TblReview entity)
        {
            _context.TblReviews.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TblReviews.FindAsync(id);
            if (entity != null)
            {
                _context.TblReviews.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
