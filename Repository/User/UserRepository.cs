using Microsoft.Extensions.Caching.Memory;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly DbBtlLtwncContext _context;

    public UserRepository(DbBtlLtwncContext context)
    {
        _context = context;
    }

    public TblUser FindMember(string memberName, string memberPassword)
    {
        return _context.TblUsers
            .FirstOrDefault(m => m.SEmail == memberName && m.SPassword == memberPassword);
    }

    // Lấy danh sách tất cả các thành viên
    public List<TblUser> GetMembers()
    {
        return _context.TblUsers.ToList();
    }

    public TblUser Update(TblUser user)
    {
        try
        {
            var existingUser = _context.TblUsers.FirstOrDefault(u => u.SEmail == user.SEmail);
            if (existingUser != null)
            {
                existingUser.SFullName = user.SFullName;
                existingUser.SPhoneNumber = user.SPhoneNumber;

                _context.SaveChanges();
                return existingUser;
            }
            return null;
        }
        catch (Exception ex)
        {
            // Log exception
            return null;
        }
    }

    public bool IsEmailExists(string email)
    {
        return _context.TblUsers.Any(u => u.SEmail == email);
    }

    public TblUser Add(TblUser user)
    {
        try
        {
            _context.TblUsers.Add(user);
            _context.SaveChanges();
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public bool Delete(string email)
    {
        try
        {
            var user = _context.TblUsers.Find(email);
            if (user != null)
            {
                _context.TblUsers.Remove(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public TblUser GetAccount(int userId)
        {
            // Tìm tài khoản người dùng trong TblUser theo IUserId
            var user = _context.TblUsers
                .Include(u => u.TblAuctions) 
                .Include(u => u.TblBids)
                .Include(u => u.TblProducts)
                .Include(u => u.TblReviews)
                .Include(u => u.TblTransactions)
                .Include(u => u.TblWatchlists)
                .FirstOrDefault(u => u.IUserId == userId); 

            return user;
        }
    public bool ChangePassword(string email, string newPassword)
    {
        try
        {
            var user = _context.TblUsers.Find(email);
            if (user != null)
            {
                user.SPassword = newPassword;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public TblUser GetByEmail(string email)
    {
        return _context.TblUsers.FirstOrDefault(u => u.SEmail == email);
    }

}
