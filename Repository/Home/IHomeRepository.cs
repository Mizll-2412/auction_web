using System.Collections.Generic;
using System.Threading.Tasks;
using BTL_LTWNC.Models;  // Giả sử đây là namespace của các model bạn sử dụng

namespace BTL_LTWNC.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<TblCategory>> GetCategoriesAsync();
        Task<IEnumerable<TblAuction>> GetUpcomingAuctionsAsync();
        Task<IEnumerable<TblAuction>> GetActiveAuctionsAsync();
        Task<IEnumerable<TblAuction>> GetPastAuctionsAsync();
    }
}
