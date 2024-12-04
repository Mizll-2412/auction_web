using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BTL_LTWNC.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DbBtlLtwncContext _context;

        public AdminRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }

        public TblUser Delete(TblUser account)
        {
            if (account.SRole == "Người dùng")
            {
                // Xóa các sản phẩm của Employer
                var products = _context.TblProducts.Where(p => p.ISellerId == account.IUserId).ToList();
                foreach (var product in products)
                {
                    _context.TblProducts.Remove(product);
                }

                // Xóa các cuộc đấu giá của Employer
                var auctions = _context.TblAuctions.Where(a => a.IWinnerId == account.IUserId).ToList();
                foreach (var auction in auctions)
                {
                    _context.TblAuctions.Remove(auction);
                }

                // Xóa các giao dịch của Employer
                var transactions = _context.TblTransactions.Where(t => t.IBuyerId == account.IUserId).ToList();
                foreach (var transaction in transactions)
                {
                    _context.TblTransactions.Remove(transaction);
                }

                // Xóa các sản phẩm trong danh sách theo dõi của Employer
                var watchlists = _context.TblWatchlists.Where(w => w.IUserId == account.IUserId).ToList();
                foreach (var watchlist in watchlists)
                {
                    _context.TblWatchlists.Remove(watchlist);
                }
            }
            _context.TblUsers.Remove(account);

            _context.SaveChanges();

            return account;
        }
        public TblUser GetAccount(int userId)
        {
            // Tìm tài khoản người dùng trong TblUser theo IUserId
            var user = _context.TblUsers
                .FirstOrDefault(u => u.IUserId == userId); // Tìm người dùng đầu tiên với userId trùng khớp

            return user;
        }
        public void Update(TblUser user)
        {
            var existingUser = _context.TblUsers
                                         .Where(u => u.IUserId == user.IUserId)
                                         .FirstOrDefault();

            if (existingUser != null)
            {
                existingUser.SFullName = user.SFullName;
                existingUser.SEmail = user.SEmail;
                existingUser.SRole = user.SRole;
                _context.Entry(existingUser).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public TblUser AddUser(TblUser newUser)
        {
            // Giả sử bạn sử dụng Entity Framework, thêm người dùng vào DbContext và lưu vào cơ sở dữ liệu.
            _context.TblUsers.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }
    }
}
