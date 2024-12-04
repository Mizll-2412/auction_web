using BTL_LTWNC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BTL_LTWNC.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public readonly DbBtlLtwncContext _context;
        public AccountRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }
        public async Task<TblUser?> LoginAsync(string email, string password){
            return await _context.TblUsers.FirstOrDefaultAsync(a => a.SEmail == email && a.SPassword == password);
        }
        public async Task<bool> RegisterAsync(TblUser tblUser)
        {
            if(await IsEmailExistAsync(tblUser.SEmail))
            {
                return false;
            }

            _context.TblUsers.Add(tblUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.TblUsers.AnyAsync(a => a.SEmail == email);
        }

    }
}