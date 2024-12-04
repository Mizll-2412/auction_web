using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface IUserRepository
    {
        public List<TblUser> GetMembers();
        public TblUser FindMember(string memberName, string memberPassword);
        TblUser Update(TblUser user);
        bool IsEmailExists(string email);
        TblUser Add(TblUser user);

        bool Delete(string email);

        bool ChangePassword(string email, string newPassword);
        TblUser GetAccount(int userId);
        TblUser GetByEmail(string email);
    }
}