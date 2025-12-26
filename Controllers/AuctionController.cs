using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_LTWNC.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly DbBtlLtwncContext _context;

        public AuctionController(IAuctionRepository auctionRepository
        , IBidRepository bidRepository, ITransactionRepository transactionRepository, DbBtlLtwncContext context)

        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _transactionRepository = transactionRepository;
            _context = context;

        }

        // // GET: Auction/Auction/{id}
        // public async Task<IActionResult> Auction(int id)
        // {
        //     // Kiểm tra session của người dùng
        //     var userJson = HttpContext.Session.GetString("UserSession");
        //     if (string.IsNullOrEmpty(userJson))
        //     {
        //         return RedirectToAction("Login", "Account");  // Nếu không đăng nhập, chuyển đến trang login
        //     }

        //     var user = JsonConvert.DeserializeObject<TblUser>(userJson);
        //     ViewBag.FullName = user?.SFullName;
        //     ViewBag.UserId = user?.IUserId;

        //     var auction = await _auctionRepository.GetByIdAsync(id);
        //     if (auction == null)
        //     {
        //         return NotFound(); // Nếu không tìm thấy đấu giá, trả về lỗi 404
        //     }

        //     var auctionBids = await _bidRepository.GetBidsByAuctionIdAsync(id);

        //     ViewBag.Auction = auction;
        //     ViewBag.AuctionBids = auctionBids;

        //     return View();
        // }
        [Route("Auction/Detail")]
        public async Task<IActionResult> Auction(int productId)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var auction = await _auctionRepository.GetAuctionByProductIdAsync(productId);
                if (auction == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy phiên đấu giá cho sản phẩm này.";
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Auction = auction;
                var bids = await _bidRepository.GetBidsByAuctionIdAsync(auction.IAuctionId);
                ViewBag.AuctionBid = bids;
                var transactions = await _transactionRepository.GetByIdAsync(auction.IAuctionId);
                ViewBag.AuctionTransactions = transactions;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PlaceBid(int auctionId, decimal bidAmount)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập." });
            }

            var user = JsonConvert.DeserializeObject<TblUser>(userJson);

            var auction = await _context.TblAuctions.FindAsync(auctionId);

            if (auction == null)
            {
                return Json(new { success = false, message = "Phiên đấu giá không tồn tại." });
            }

            if (DateTime.Now < auction.DtStartTime)
            {
                return Json(new { success = false, message = "Chưa bắt đầu." });
            }

            if (DateTime.Now > auction.DtEndTime)
            {
                return Json(new { success = false, message = "Đã kết thúc." });
            }

            if (bidAmount <= (auction.DHighestBid ?? 0))
            {
                return Json(new { success = false, message = $"Phải cao hơn {auction.DHighestBid:N0} VNĐ" });
            }

            var bid = new TblBid
            {
                IAuctionId = auctionId,
                IBidderId = user.IUserId,
                DBidAmount = bidAmount,
                DtBidTime = DateTime.Now
            };
            _context.TblBids.Add(bid);

            auction.DHighestBid = bidAmount;
            _context.TblAuctions.Update(auction);

            _context.TblTransactions.Add(new TblTransaction
            {
                IAuctionId = auctionId,
                IBuyerId = user.IUserId,
                DAmount = bidAmount,
                DtTransactionTime = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Thành công!", newHighestBid = bidAmount });
        }
        [HttpGet]
        public async Task<IActionResult> CreateAuction(int productId)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = JsonConvert.DeserializeObject<TblUser>(userJson);
            if (user.SRole != "Quản trị viên")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var product = await _context.TblProducts
                .Include(p => p.ISeller)
                .FirstOrDefaultAsync(p => p.IProductId == productId);
                if (product == null)
                {
                    return NotFound();
                }
                var existingAuction = await _context.TblAuctions
                .FirstOrDefaultAsync(a => a.IProductId == productId && a.SStatus != "Đã kết thúc");
                if (existingAuction != null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm này đã có cuộc đấu giá đang diễn ra";
                    return RedirectToAction("ListProduct", "Product", new { categoryId = product.ICategoryId });
                }
                ViewBag.Product = product;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuction(TblAuction auction)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var user = JsonConvert.DeserializeObject<TblUser>(userJson);
                if (auction.DtEndTime <= auction.DtStartTime)
                {
                    ViewBag.ErrorMessage = "Thời gian kết thúc phải sau thời gian bắt đầu";
                    ViewBag.Product = await _context.TblProducts
                    .Include(p => p.ISeller)
                    .FirstOrDefaultAsync(p => p.IProductId == auction.IProductId);
                    return View(auction);
                }
                var product = await _context.TblProducts.FindAsync(auction.IProductId);
                if (product == null)
                {
                    return NotFound();
                }
                if (auction.DHighestBid == null || auction.DHighestBid == 0)
                {
                    auction.DHighestBid = product.DStartingPrice;
                }
                _context.TblAuctions.Add(auction);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Tạo đấu giá thành công";
                return RedirectToAction("ListProduct", "Product", new { categoryId = product.ICategoryId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.Product = await _context.TblProducts
                    .Include(p => p.ISeller)
                    .FirstOrDefaultAsync(p => p.IProductId == auction.IProductId);
                return View(auction);
            }
        }

    }
}
