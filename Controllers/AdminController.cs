using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_LTWNC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IReviewRepository _reviewRepo;

        public AdminController(IUserRepository userRepo, IReviewRepository reviewRepository)
        {
            _userRepo = userRepo;
            _reviewRepo = reviewRepository;
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
        // DASHBOARD (Trang chủ Admin)
        // =======================
        public async Task<IActionResult> DashboardAsync()
        {
            var check = CheckLoginAndRole();

            if (check != null) return check;

            ViewBag.Role = HttpContext.Session.GetString("Role");

            // TODO: Thêm logic thống kê
            ViewBag.TotalUsers = _userRepo.GetMembers().Count();
            var reviews = await _reviewRepo.GetAllAsync();
            ViewBag.TotalReviews = reviews.Count();
            return View();
        }

        // =======================
        // USERS MANAGEMENT
        // =======================
        public IActionResult Users()
        {
            var check = CheckLoginAndRole();
            if (check != null) return check;

            ViewBag.Role = HttpContext.Session.GetString("Role");
            var users = _userRepo.GetMembers();
            return View(users);
        }

        // Giữ nguyên để backward compatible
        public IActionResult UserList()
        {
            return RedirectToAction("Users");
        }

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

        [HttpPost]
        public IActionResult AddUser([FromBody] TblUser user)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            if (string.IsNullOrEmpty(user.SEmail) || string.IsNullOrEmpty(user.SPassword))
                return Json(new { success = false, message = "Thiếu dữ liệu" });

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

        // =======================
        // POSTS MANAGEMENT
        // =======================
        public IActionResult Posts()
        {
            var check = CheckLoginAndRole();
            if (check != null) return check;

            ViewBag.Role = HttpContext.Session.GetString("Role");

            // TODO: Lấy danh sách posts từ repository/dbcontext
            var posts = new List<object>(); // Tạm thời trả về empty

            return View(posts);
        }

        // =======================
        // CATEGORIES MANAGEMENT
        // =======================
        public IActionResult Categories()
        {
            var check = CheckLoginAndRole();
            if (check != null) return check;

            ViewBag.Role = HttpContext.Session.GetString("Role");

            // TODO: Lấy danh sách categories
            var categories = new List<object>();

            return View(categories);
        }

        // =======================
        // PRODUCTS MANAGEMENT
        // =======================
        public IActionResult Products()
        {
            var check = CheckLoginAndRole();
            if (check != null) return check;

            ViewBag.Role = HttpContext.Session.GetString("Role");

            // TODO: Lấy danh sách products
            var products = new List<object>();

            return View(products);
        }

        // =======================
        // REVIEWS MANAGEMENT
        // =======================
        public IActionResult Reviews()
        {
            var check = CheckLoginAndRole();
            if (check != null) return check;

            ViewBag.Role = HttpContext.Session.GetString("Role");
            var reviews = _reviewRepo.GetAllAsync();

            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewDetail(int id)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            try
            {
                var review = await _reviewRepo.GetByIdAsync(id);

                if (review == null)
                    return Json(new { success = false, message = "Không tìm thấy đánh giá" });

                return Json(new
                {
                    success = true,
                    review = new
                    {
                        reviewId = review.IReviewId,
                        productName = review.IProduct?.SProductName ?? "N/A",
                        reviewerName = review.IReviewer?.SFullName ?? "N/A",
                        reviewerEmail = review.IReviewer?.SEmail ?? "N/A",
                        rating = review.IRating ?? 0,
                        comment = review.SComment ?? "",
                        reviewTime = review.DtReviewTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Không có quyền" });

            try
            {
                var review = await _reviewRepo.GetByIdAsync(id);
                if (review == null)
                    return Json(new { success = false, message = "Không tìm thấy đánh giá" });

                await _reviewRepo.DeleteAsync(id);

                return Json(new { success = true, message = "Xóa đánh giá thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
    }
}