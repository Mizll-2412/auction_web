using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_LTWNC.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;

        public AuctionController(IAuctionRepository auctionRepository, IBidRepository bidRepository)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
        }

        // GET: Auction/Auction/{id}
        public async Task<IActionResult> Auction(int id)
        {
            // Kiểm tra session của người dùng
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");  // Nếu không đăng nhập, chuyển đến trang login
            }

            var user = JsonConvert.DeserializeObject<TblUser>(userJson);
            ViewBag.FullName = user?.SFullName;
            ViewBag.UserId = user?.IUserId;

            var auction = await _auctionRepository.GetByIdAsync(id);
            if (auction == null)
            {
                return NotFound(); // Nếu không tìm thấy đấu giá, trả về lỗi 404
            }

            var auctionBids = await _bidRepository.GetBidsByAuctionIdAsync(id);

            ViewBag.Auction = auction;
            ViewBag.AuctionBids = auctionBids;

            return View();
        }
    }
}
