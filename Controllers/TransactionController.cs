using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BTL_LTWNC.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWatchlistRepository _watchlistRepository; // Giả sử bạn có repository cho Watchlist

        // Use a single constructor to accept all required dependencies
        public TransactionController(
            ITransactionRepository transactionRepository,
            IProductRepository productRepository,
            IWatchlistRepository watchlistRepository)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _watchlistRepository = watchlistRepository;
        }

        // Chức năng lấy thông tin người dùng từ session
        public TblUser GetUserFromSession()
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (!string.IsNullOrEmpty(userJson))
            {
                return JsonConvert.DeserializeObject<TblUser>(userJson);
            }
            return null;
        }

        // GET: ThanhToan từ Watchlist
        public async Task<IActionResult> ThanhToan(int watchlistId)
        {
            // Lấy thông tin người dùng từ session
            var user = GetUserFromSession();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin sản phẩm từ bảng Watchlist theo watchlistId
            var watchlist = await _watchlistRepository.GetByIdAsync(watchlistId);
            // if (watchlist == null || watchlist.IProduct == null)
            // {
            //     return RedirectToAction("Watchlist", "Watchlist");
            // }
            Console.WriteLine($"Watchlist: {watchlist}");

            var product = watchlist.IProduct;

            ViewBag.FullName = user.SFullName;
            ViewBag.Email = user.SEmail;
            ViewBag.PhoneNumber = user.SPhoneNumber;
            ViewBag.Product = product;  // Truyền thông tin sản phẩm vào View

            // Trả về view thanh toán
            return View("~/Views/Transaction/ThanhToan.cshtml");
        }

        // GET: GiaoHang
        public IActionResult GiaoHang()
        {
            var user = GetUserFromSession();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.FullName = user.SFullName;
            ViewBag.Email = user.SEmail;
            ViewBag.PhoneNumber = user.SPhoneNumber;

            return View("GiaoHang");
        }
    }
}
