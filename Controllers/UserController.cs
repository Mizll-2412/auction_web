using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;

namespace BTL_LTWNC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("User");
        }
        [HttpPost]
        public IActionResult Edit(TblUser updatedUser)
        {
            try
            {
                var email = HttpContext.Session.GetString("SEmail");
                if (string.IsNullOrEmpty(email))
                {
                    return Json(new { success = false, message = "Phiên đăng nhập đã hết hạn." });
                }

                var currentUser = _userRepository.GetByEmail(email);
                if (currentUser == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin người dùng." });
                }

                // Cập nhật thông tin
                currentUser.SFullName = updatedUser.SFullName;
                currentUser.SPhoneNumber = updatedUser.SPhoneNumber;

                var result = _userRepository.Update(currentUser);
                if (result != null)
                {
                    // Cập nhật lại session
                    HttpContext.Session.SetString("FullName", result.SFullName);
                    HttpContext.Session.SetString("Phone", result.SPhoneNumber ?? "");

                    return Json(new { success = true, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật thông tin thất bại." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
