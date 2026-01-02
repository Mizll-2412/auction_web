using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using BTL_LTWNC.Repositories.Notification;

namespace BTL_LTWNC.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly NotificationService _notificationService;
        private readonly INotificationRepository _notificationRepo;

        private readonly DbBtlLtwncContext _context;

        public AuctionController(IAuctionRepository auctionRepository
        , IBidRepository bidRepository, ITransactionRepository transactionRepository
        , DbBtlLtwncContext context, NotificationService notificationService, INotificationRepository notificationRepo)

        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _transactionRepository = transactionRepository;
            _context = context;
            _notificationService = notificationService;
            _notificationRepo = notificationRepo;

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
            var previousHighestBid = await _context.TblBids
                                    .Include(b => b.IBidder)
                                    .Where(b => b.IAuctionId == auctionId)
                                    .OrderByDescending(b => b.DBidAmount)
                                    .FirstOrDefaultAsync();

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
            if (auction.IProduct?.ISellerId != null && auction.IProduct.ISellerId != user.IUserId)
            {
                await _notificationService.NotifyNewBid(
                  sellerId: auction.IProduct.ISellerId.Value,
                  bidderId: user.IUserId,
                  auctionId: auctionId,
                  productId: auction.IProductId.Value,
                  bidderName: user.SFullName,
                  bidAmount: bidAmount,
                  productName: auction.IProduct.SProductName
              );
            }
            if (previousHighestBid != null && previousHighestBid.IBidderId != user.IUserId)
            {
                await _notificationService.NotifyOutbid(
                    oldBidderId: previousHighestBid.IBidderId.Value,
                    newBidderId: user.IUserId,
                    auctionId: auctionId,
                    productId: auction.IProductId.Value,
                    newBidderName: user.SFullName,
                    newBidAmount: bidAmount,
                    productName: auction.IProduct.SProductName
                );
            }

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
                if (product.ISellerId != null)
                {
                    var notification = new TblNotification
                    {
                        iUserId = product.ISellerId.Value,
                        iSenderId = user.IUserId,
                        iAuctionId = auction.IAuctionId,
                        iProductId = product.IProductId,
                        sTitle = "Phiên đấu giá mới được tạo",
                        SMessage = $"Sản phẩm '{product.SProductName}' của bạn đã được đưa vào đấu giá. Bắt đầu: {auction.DtStartTime:dd/MM/yyyy HH:mm}",
                        SType = "auction_started",
                        SUrl = $"/Auction/Detail?productId={product.IProductId}"
                    };
                    await _notificationRepo.CreateNotificationAsync(notification);

                }
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

        [HttpPost]
        public async Task<IActionResult> EndAuctionEarly(int auctionId)
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
                var auction = await _context.TblAuctions
           .Include(a => a.IProduct)
           .FirstOrDefaultAsync(a => a.IAuctionId == auctionId);

                if (auction == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiên đấu giá." });
                }

                if (auction.SStatus == "Đã kết thúc")
                {
                    return Json(new { success = false, message = "Phiên đấu giá đã kết thúc rồi." });
                }
                var highestBid = await _context.TblBids
                    .Where(b => b.IAuctionId == auctionId)
                    .OrderByDescending(b => b.DBidAmount)
                    .FirstOrDefaultAsync();

                auction.SStatus = "Đã kết thúc";
                auction.DtEndTime = DateTime.Now;

                if (highestBid != null)
                {
                    auction.DHighestBid = highestBid.DBidAmount;
                    auction.IWinnerId = highestBid.IBidderId;
                }
                else
                {
                    auction.DHighestBid = auction.IProduct?.DStartingPrice ?? 0;
                    auction.IWinnerId = null;
                }

                await _context.SaveChangesAsync();
                if (highestBid != null && highestBid.IBidderId != null)
                {
                    await _notificationService.NotifyAuctionWin(
                        winnerId: highestBid.IBidderId.Value,
                        sellerId: auction.IProduct?.ISellerId ?? 0,
                        auctionId: auctionId,
                        productId: auction.IProductId ?? 0,
                        productName: auction.IProduct?.SProductName ?? "Sản phẩm",
                        finalPrice: highestBid.DBidAmount ?? 0
                    );

                    if (auction.IProduct?.ISellerId != null)
                    {
                        await _notificationService.NotifyProductSold(
                            sellerId: auction.IProduct.ISellerId.Value,
                            winnerId: highestBid.IBidderId.Value,
                            auctionId: auctionId,
                            productId: auction.IProductId ?? 0,
                            winnerName: highestBid.IBidder?.SFullName ?? "Người mua",
                            productName: auction.IProduct?.SProductName ?? "Sản phẩm",
                            finalPrice: highestBid.DBidAmount ?? 0
                        );
                    }
                    var losingBidders = await _context.TblBids
                                        .Include(b => b.IBidder)
                                        .Where(b => b.IAuctionId == auctionId && b.IBidderId != highestBid.IBidderId)
                                        .Select(b => b.IBidderId)
                                        .Distinct()
                                        .ToListAsync();
                    var losingNotifications = new List<TblNotification>();
                    foreach (var loserId in losingBidders)
                    {
                        if (loserId != null)
                        {
                            losingNotifications.Add(new TblNotification
                            {
                                iUserId = loserId.Value,
                                iSenderId = highestBid.IBidderId,
                                iAuctionId = auctionId,
                                iProductId = auction.IProductId ?? 0,
                                sTitle = "Phiên đấu giá đã kết thúc",
                                SMessage = $"Rất tiếc! Bạn đã không thắng đấu giá '{auction.IProduct?.SProductName}'. Người thắng: {highestBid.IBidder?.SFullName}",
                                SType = "auction_lost",
                                SUrl = $"/Auction/Detail?productId={auction.IProductId}"
                            });
                        }
                    }
                    if (losingNotifications.Any())
                    {
                        await _notificationRepo.CreateNotificationsAsync(losingNotifications);
                    }
                }
                else
                {
                    if(auction.IProduct?.ISellerId != null)
                    {
                        var notification = new TblNotification
                        {
                            iUserId = auction.IProduct.ISellerId.Value,
                            iAuctionId = auctionId,
                            iProductId = auction.IProductId ?? 0,
                            sTitle = "Đấu giá kết thúc không có người thắng",
                            SMessage = $"Phiên đấu giá '{auction.IProduct?.SProductName}' đã kết thúc nhưng không có ai đặt giá.",
                            SType = "auction_no_winner",
                            SUrl = $"/Auction/Detail?productId={auction.IProductId}"
                        };
                        await _notificationRepo.CreateNotificationAsync(notification);
                    }
                }

                string message = highestBid != null
                    ? $"Đã kết thúc đấu giá sớm! Người thắng cuộc: {highestBid.IBidder?.SFullName}"
                    : "Đã kết thúc đấu giá sớm! Không có người thắng cuộc.";

                return Json(new { success = true, message = message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }

}

