using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly DbBtlLtwncContext _context;

        public HomeRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TblCategory>> GetCategoriesAsync()
        {
            return await _context.TblCategories.ToListAsync();
        }

        public async Task<IEnumerable<TblAuction>> GetUpcomingAuctionsAsync()
        {
            var now = DateTime.Now;
            var upcomingAuctions = await _context.TblAuctions
                .Include(a => a.IProduct) // Bao gồm thông tin sản phẩm liên quan
                .Where(a => a.DtStartTime > now) // Các đấu giá sắp diễn ra
                .ToListAsync();
            return upcomingAuctions;
        }
        public async Task<IEnumerable<TblAuction>> GetActiveAuctionsAsync()
        {
            var now = DateTime.Now;
            var activeAuctions = await _context.TblAuctions
                .Include(a => a.IProduct) // Bao gồm thông tin sản phẩm liên quan
                .Where(a => a.DtStartTime < now && a.DtEndTime > now) // Các đấu giá đang diễn ra
                .ToListAsync();
            return activeAuctions;
        }


        public async Task<IEnumerable<TblAuction>> GetPastAuctionsAsync()
        {
            var now = DateTime.Now;
            var pastAuctions = await _context.TblAuctions
                .Include(a => a.IProduct) // Bao gồm thông tin sản phẩm liên quan
                .Where(a => a.DtEndTime < now) // Các đấu giá đã kết thúc
                .ToListAsync();
            return pastAuctions;
        }
    }
}
