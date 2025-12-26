using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;

namespace BTL_LTWNC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _homeRepository;

        public HomeController(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _homeRepository.GetCategoriesAsync();
            var upcomingAuctions = await _homeRepository.GetUpcomingAuctionsAsync();
            var activeAuctions = await _homeRepository.GetActiveAuctionsAsync();
            var pastAuctions = await _homeRepository.GetPastAuctionsAsync();
            ViewBag.Categories = categories;    
            ViewBag.UpcomingAuctions = upcomingAuctions;
            ViewBag.CurrentAuctions = activeAuctions;
            ViewBag.PastAuctions = pastAuctions;
            return View();
        }
    }
}
