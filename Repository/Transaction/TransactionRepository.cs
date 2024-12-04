using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DbBtlLtwncContext _context;

        public TransactionRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TblTransaction>> GetAllAsync()
        {
            return await _context.TblTransactions
                .Include(p => p.IAuctionId)
                .Include(p => p.IBuyerId)
                .ToListAsync();
        }

        public async Task<TblTransaction> GetByIdAsync(int id)
        {
            return await _context.TblTransactions.FindAsync(id);
        }

        public async Task AddAsync(TblTransaction entity)
        {
            await _context.TblTransactions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TblTransaction entity)
        {
            _context.TblTransactions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TblTransactions.FindAsync(id);
            if (entity != null)
            {
                _context.TblTransactions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
