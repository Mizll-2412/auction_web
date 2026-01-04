using BTL_LTWNC.Extensions;
using BTL_LTWNC.Models;
using BTL_LTWNC.Models.ViewModels;
using BTL_LTWNC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace BTL_LTWNC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // ================= LOGIN AJAX =================
        [HttpPost]
        public async Task<IActionResult> LoginAjax([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .First().ErrorMessage;

                return Json(new { success = false, message = error });
            }

            // Hash mật khẩu trước khi so sánh
            string hashedPassword = HashPassword(model.SPassword);
            TblUser user = await _accountRepository.LoginAsync(model.SEmail, hashedPassword);

            if (user == null)
                return Json(new { success = false, message = "Sai email hoặc mật khẩu" });

            HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(user));
            HttpContext.Session.SetString("SEmail", user.SEmail);
            HttpContext.Session.SetInt32("UserId", user.IUserId);
            HttpContext.Session.SetString("FullName", user.SFullName);
            HttpContext.Session.SetString("Role", user.SRole);


            return Json(new { success = true });
        }

        // ================= REGISTER AJAX =================
        [HttpPost]
        public async Task<IActionResult> RegisterAjax([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .First().ErrorMessage;

                return Json(new { success = false, message = error });
            }

            // Kiểm tra mật khẩu xác nhận
            if (model.SPassword != model.RePassword)
                return Json(new { success = false, message = "Mật khẩu xác nhận không khớp" });

            if (await _accountRepository.IsEmailExistAsync(model.SEmail))
                return Json(new { success = false, message = "Email đã tồn tại" });

            TblUser user = new TblUser
            {
                SEmail = model.SEmail,
                SFullName = model.SFullName,
                SPhoneNumber = model.SPhoneNumber,
                SPassword = HashPassword(model.SPassword),
                VerifyKey = GenerateVerifyKey(),
                SRole = "Người dùng"
            };

            await _accountRepository.RegisterAsync(user);

            return Json(new { success = true });
        }

        // ================= LOGOUT =================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ================= HASH PASSWORD (SHA256) =================
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // ================= VERIFY KEY =================
        private string GenerateVerifyKey()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }
    }
}