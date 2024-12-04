using BTL_LTWNC.Extensions;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BTL_LTWNC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            int loginAttempts = HttpContext.Session.GetInt32("LoginAttempts") ?? 0;
            DateTime? lockoutEnd = HttpContext.Session.Get<DateTime?>("LockoutEnd");

            if (lockoutEnd.HasValue && lockoutEnd > DateTime.Now)
            {
                TimeSpan remainingLockout = lockoutEnd.Value - DateTime.Now;
                ModelState.AddModelError("", $"Bạn đã nhập sai mật khẩu 3 lần. Vui lòng thử lại sau {remainingLockout.Minutes} phút {remainingLockout.Seconds} giây.");
                return View();
            }

            TblUser tblUser = await _accountRepository.LoginAsync(email, password);
            if (tblUser == null)
            {
                loginAttempts++;
                HttpContext.Session.SetInt32("LoginAttempts", loginAttempts);

                if (loginAttempts >= 3)
                {
                    lockoutEnd = DateTime.Now.AddMinutes(30);
                    HttpContext.Session.Set("LockoutEnd", lockoutEnd);
                    HttpContext.Session.SetInt32("LoginAttempts", 0);
                    ModelState.AddModelError("", "Bạn đã nhập sai mật khẩu 3 lần. Vui lòng thử lại sau 30 phút.");
                    return View();
                }
                else
                {
                    int remainingAttempts = 3 - loginAttempts;
                    ModelState.AddModelError("", $"Tên đăng nhập hoặc mật khẩu không đúng. Bạn còn {remainingAttempts} lần thử.");
                    return View();
                }
            }
            else
            {
                // Đăng nhập thành công
                HttpContext.Session.SetInt32("LoginAttempts", 0); // Đặt lại số lần thử
                HttpContext.Session.Remove("LockoutEnd"); // Xóa thời gian khóa nếu có

                var userJson = JsonConvert.SerializeObject(tblUser);
                HttpContext.Session.SetString("UserSession", userJson);

                // Lưu một số thông tin khác nếu cần (không bắt buộc)
                HttpContext.Session.SetString("SEmail", tblUser.SEmail);
                HttpContext.Session.SetString("FullName", tblUser.SFullName);
                HttpContext.Session.SetString("Phone", tblUser.SPhoneNumber);
                HttpContext.Session.SetString("Role", tblUser.SRole);


                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(TblUser user)
        {
            if (await _accountRepository.RegisterAsync(user))
            {
                return RedirectToAction("Login"); 
            }
            ViewBag.ErrorMessage = "Email already exists.";
            return View(user); 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("SEmail");
            HttpContext.Session.Remove("FullName");
            HttpContext.Session.Remove("Phone");
            return RedirectToAction("Index", "Home");
        }
    }
}
