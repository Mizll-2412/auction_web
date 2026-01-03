using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using System.Linq;

namespace BTL_LTWNC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepo;

        public AdminController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // =======================
        // CHECK LOGIN + ROLE
        // =======================
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Quản trị viên";
        }

        private IActionResult CheckLoginAndRole()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (!IsAdmin())
                return Forbid();

            return null;
        }

        // =======================
        // VIEW: USER LIST
        // =======================
        public IActionResult UserList()
        {
            var check = CheckLoginAndRole();
            if (check != null) return check;

            ViewBag.Role = HttpContext.Session.GetString("Role");
            var users = _userRepo.GetMembers();
            return View(users);
        }

        // =======================
        // GET USER DETAIL (AJAX)
        // =======================
        [HttpGet]
        public IActionResult GetUserDetails(int userId)
        {
            if (!IsAdmin())
                return Json(new { success = false });

            var user = _userRepo.GetAccount(userId);
            if (user == null)
                return Json(new { success = false });

            return Json(new
            {
                success = true,
                user = new
                {
                    iUserId = user.IUserId,
                    sFullName = user.SFullName,
                    sEmail = user.SEmail,
                    sRole = user.SRole
                }
            });
        }

        // =======================
        // ADD USER (AJAX)
        // =======================
        [HttpPost]
        public IActionResult AddUser([FromBody] TblUser user)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            if (string.IsNullOrEmpty(user.SEmail) || string.IsNullOrEmpty(user.SPassword))
                return Json(new { success = false, message = "Thiếu dữ liệu" });

            // ⚠️ DEMO: nên hash password ở đây
            _userRepo.Add(user);

            return Json(new
            {
                success = true,
                user = new
                {
                    user.IUserId,
                    user.SFullName,
                    user.SEmail,
                    user.SRole,
                }
            });
        }

        // =======================
        // UPDATE USER (AJAX)
        // =======================
        [HttpPost]
        public IActionResult UpdateUser([FromBody] TblUser model)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            var user = _userRepo.GetAccount(model.IUserId);
            if (user == null)
                return Json(new { success = false, message = "Không tìm thấy user" });

            user.SFullName = model.SFullName;
            user.SEmail = model.SEmail;
            user.SRole = model.SRole;

            _userRepo.Update(user);

            return Json(new { success = true });
        }

        // =======================
        // DELETE USER (AJAX)
        // =======================
        [HttpDelete]
        public IActionResult DeleteUserAJAX(int userId)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            var user = _userRepo.GetAccount(userId);
            if (user == null)
                return Json(new { success = false, message = "Không tìm thấy user" });

            _userRepo.Delete(user.SEmail);

            return Json(new { success = true });
        }
    }
}
