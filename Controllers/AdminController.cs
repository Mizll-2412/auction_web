using System.Text.Json;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;  // Thêm dòng này


namespace BTL_LTWNC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly DbBtlLtwncContext _context;
        private readonly IReviewRepository _reviewRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWatchlistRepository _watchlistRepository;

        public AdminController(IProductRepository productRepository, IAuctionRepository auctionRepository, DbBtlLtwncContext context,
               IReviewRepository reviewRepository, ITransactionRepository transactionRepository,
                IUserRepository userRepository, IWatchlistRepository watchlistRepository, IAdminRepository adminRepository)
        {
            _productRepository = productRepository;
            _auctionRepository = auctionRepository;
            _context = context;
            _reviewRepository = reviewRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _watchlistRepository = watchlistRepository;
            _adminRepository = adminRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DSUser()
        {
            var role = HttpContext.Session.GetString("Role");
            Console.WriteLine("User role from session: " + role); // Kiểm tra giá trị role trong console

            ViewBag.Role = role;
            var lstUser = _userRepository.GetMembers();
            return View(lstUser);
        }

        // public IActionResult DeleteUser(int userId)
        // {
        //     var user = _userRepository.GetAccount(userId);
        //     if (user != null)
        //     {
        //         // Tiến hành xóa tài khoản
        //         _userRepository.Delete(user);

        //         // Lưu thông báo vào TempData để hiển thị trong view
        //         TempData["SuccessMessage"] = "Tài khoản đã được xóa thành công!";
        //         RedirectToAction("DSUser", "Admin");
        //     }
        //     else
        //     {
        //         // Nếu không tìm thấy người dùng
        //         TempData["ErrorMessage"] = "Tài khoản không tồn tại!";
        //         RedirectToAction("DSUser", "Admin");
        //     }

        //     // Điều hướng lại về trang danh sách người dùng hoặc trang chính
        //     return RedirectToAction("DSUser", "Admin");
        // }

        [HttpDelete]
        public IActionResult DeleteUserAJAX(int userId)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            try
            {
                var user = _userRepository.GetAccount(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy user!" });
                }
                _adminRepository.Delete(user);
                return Json(new { success = true, message = "Xóa user và các liên kết liên quan thành công!" });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting house: {ex.Message}");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa nhà user!" });
            }
        }
        public IActionResult GetUserDetails(int userId)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            var user = _adminRepository.GetAccount(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found!" });
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };

            return Json(new { success = true, user });
        }
        [HttpPost]
        public IActionResult UpdateUser([FromBody] TblUser user)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            if (user == null || user.IUserId <= 0)
            {
                return Json(new { success = false, message = "Invalid user data!" });
            }

            // Tìm người dùng từ cơ sở dữ liệu bằng userId
            var existingUser = _adminRepository.GetAccount(user.IUserId);
            if (existingUser == null)
            {
                return Json(new { success = false, message = "User not found!" });
            }

            try
            {
                existingUser.SFullName = user.SFullName;
                existingUser.SEmail = user.SEmail;
                existingUser.SRole = user.SRole;

                // Lưu các thay đổi vào cơ sở dữ liệu
                _adminRepository.Update(existingUser);

                // Trả về kết quả thành công
                return Json(new { success = true, message = "User updated successfully!" });
            }
            catch (Exception ex)
            {
                // Nếu có lỗi khi cập nhật, trả về thông báo lỗi
                return Json(new { success = false, message = "Error updating user: " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult AddUser([FromBody] TblUser newUser)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            if (!Regex.IsMatch(newUser.VerifyKey,@"^[A-Za-z0-9]{5}\d$"))
            {
            return Json(new { success = false, message = "VerifyKey không hợp lệ. Nó phải có 6 ký tự và kết thúc bằng số." });
            }
            if (newUser == null)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }
            // var existingUser = await _context.TblUsers.FirstOrDefaultAsync(u => u.SEmail == newUser.SEmail);
            // if (existingUser != null)
            // {
            //     return Json(new { success = false, message = "Email đã tồn tại." });
            // }

            try
            {
                // Giả sử bạn có phương thức thêm người dùng trong repository
                var addedUser = _adminRepository.AddUser(newUser);
                if (addedUser != null)
                {
                    return Json(new { success = true, user = addedUser });
                }
                else
                {
                    return Json(new { success = false, message = "Thêm người dùng thất bại!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}