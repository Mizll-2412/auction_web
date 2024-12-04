using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
  public interface IAdminRepository
  {
    TblUser Delete(TblUser account);
    void Update(TblUser user);  // Thêm phương thức Update
    public TblUser GetAccount(int userId);
    TblUser AddUser(TblUser newUser);

  }
}