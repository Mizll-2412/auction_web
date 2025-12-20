using BTL_LTWNC.Extensions;
using BTL_LTWNC.Models;
using BTL_LTWNC.Models.ViewModels;
using BTL_LTWNC.Repositories;
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

            TblUser user = await _accountRepository.LoginAsync(model.SEmail, model.SPassword);

            if (user == null)
                return Json(new { success = false, message = "Sai email hoặc mật khẩu" });

            HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(user));
            HttpContext.Session.SetString("SEmail", user.SEmail);
            HttpContext.Session.SetString("FullName", user.SFullName);

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

            if (await _accountRepository.IsEmailExistAsync(model.SEmail))
                return Json(new { success = false, message = "Email đã tồn tại" });

            TblUser user = new TblUser
            {
                SEmail = model.SEmail,
                SFullName = model.SFullName,
                SPhoneNumber = model.SPhoneNumber,
                SPassword = model.SPassword,
                VerifyKey = GenerateVerifyKey(),
                SRole = "User"
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

        // ================= VERIFY KEY =================
        private string GenerateVerifyKey()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }
    }
}
