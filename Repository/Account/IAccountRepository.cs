using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface IAccountRepository
    {
        Task<TblUser?> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(TblUser tblUser);
        Task<bool> IsEmailExistAsync(string email);
        
    }
}