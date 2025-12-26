using Microsoft.AspNetCore.Mvc;
using BTL_LTWNC.Repositories;
using BTL_LTWNC.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly DbBtlLtwncContext _context;
        private readonly IReviewRepository _reviewRepository;


        public ProductController(IProductRepository productRepository, DbBtlLtwncContext context, IAuctionRepository auctionRepository
                , IReviewRepository reviewRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _auctionRepository = auctionRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<IActionResult> ListProduct(int categoryId)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (userJson.IsNullOrEmpty())
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var products = await _productRepository.GetProductsByCategory(categoryId);
                var user = JsonConvert.DeserializeObject<TblUser>(userJson);
                var myProducts = products.Where(p => p.ISellerId == user.IUserId).ToList();
                ViewBag.CategoryId = categoryId;
                
                ViewBag.IsAdmin = user.SRole == "Quản trị viên";
                return View(myProducts);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct(int categoryId)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (userJson.IsNullOrEmpty())
            {
                return RedirectToAction("Login", "Account");
            }
            var user = JsonConvert.DeserializeObject<TblUser>(userJson);
            if (user.SRole != "Quản trị viên")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Categories = await _context.TblCategories.ToListAsync();
            ViewBag.CategoryId = categoryId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(TblProduct product)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var user = JsonConvert.DeserializeObject<TblUser>(userJson);
                product.ISellerId = user.IUserId;
                await _productRepository.AddAsync(product);
                TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công!";
                return RedirectToAction("ListProduct", new { categoryId = product.ICategoryId });
            }
            catch (Exception ex)
            {
                ViewBag.Categories = await _context.TblCategories.ToListAsync();
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                return View(product);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
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
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                ViewBag.Categories = await _context.TblCategories.ToListAsync();
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(int id, TblProduct updatedProduct)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                product.SProductName = updatedProduct.SProductName;
                product.SDescription = updatedProduct.SDescription;
                product.DStartingPrice = updatedProduct.DStartingPrice;
                product.SImageUrl = updatedProduct.SImageUrl;
                product.ICategoryId = updatedProduct.ICategoryId;

                await _productRepository.UpdateAsync(product);

                TempData["SuccessMessage"] = "Sản phẩm đã được cập nhật thành công!";
                return RedirectToAction("ListProduct", new { categoryId = product.ICategoryId });
            }
            catch (Exception ex)
            {
                ViewBag.Categories = await _context.TblCategories.ToListAsync();
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                return View(updatedProduct);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetProductsByCategory(categoryId);

                var productData = products.Select(p => new
                {
                    productId = p.IProductId,
                    productName = p.SProductName,
                    price = p.DStartingPrice,
                    imageUrl = p.SImageUrl,
                    description = p.SDescription,
                    sellerName = p.ISeller != null ? p.ISeller.SFullName : "Không rõ"
                }).ToList();

                return Json(new { success = true, data = productData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }
                var auctionForProduct = await _auctionRepository.GetAuctionsByProductIdAsync(id);
                _context.TblAuctions.RemoveRange(auctionForProduct);
                var reviewsForProduct = await _reviewRepository.GetReviewsByProductIdAsync(product.IProductId);
                _context.TblReviews.RemoveRange(reviewsForProduct);
                _context.TblProducts.Remove(product);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Sản phẩm và các bản ghi liên quan đã được xóa thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa sản phẩm: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa sản phẩm. " + ex.Message });
            }
        }
    }
}
