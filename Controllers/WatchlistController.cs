using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BTL_LTWNC.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly IWatchlistRepository _watchlistRepository;

        public WatchlistController(IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }

        // GET: Watchlist
        public async Task<IActionResult> Watchlist()
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if no user session
            }

            var user = JsonConvert.DeserializeObject<TblUser>(userJson);

            // Get the user's watchlist by filtering on iUserId
            var watchlists = await _watchlistRepository.GetAllAsync();
            var userWatchlist = watchlists.Where(w => w.IUserId == user.IUserId).ToList();

            Console.WriteLine($"Number of watchlist items: {userWatchlist.Count}, UserId: {user.IUserId}");

            ViewBag.Username = user.SFullName;
            ViewBag.Role = user.SRole;

            // Pass the filtered watchlist to the view
            return View(userWatchlist);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            // Lấy thông tin user từ session
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if no user session
            }

            var user = JsonConvert.DeserializeObject<TblUser>(userJson);

            // Get the user's watchlist by filtering on iUserId
            var watchlists = await _watchlistRepository.GetAllAsync();
            var userWatchlist = watchlists.Where(w => w.IUserId == user.IUserId).ToList();
            
            ViewBag.Username = user.SFullName;
            ViewBag.Role = user.SRole;

            // Pass the filtered watchlist to the view
            return View(userWatchlist);
        }
    }
}
