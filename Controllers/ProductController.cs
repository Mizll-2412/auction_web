using Microsoft.AspNetCore.Mvc;
using BTL_LTWNC.Repositories;
using BTL_LTWNC.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace BTL_LTWNC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly DbBtlLtwncContext _context;
        private readonly IReviewRepository _reviewRepository;



        public ProductController(IProductRepository productRepository, DbBtlLtwncContext context, IAuctionRepository auctionRepository
                ,IReviewRepository reviewRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _auctionRepository = auctionRepository;
            _reviewRepository = reviewRepository;
        }

        // Danh sách sản phẩm theo danh mục
        public async Task<IActionResult> ListProduct(int categoryId)
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var products = await _productRepository.GetProductsByCategory(categoryId);
                var user = JsonConvert.DeserializeObject<TblUser>(userJson);
                ViewBag.CategoryId = categoryId;
                ViewBag.IsAdmin = user.SRole == "Quản trị viên";  // Nếu vai trò là "Quản trị viên" thì xem như Admin
                return View(products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }


        // API lấy sản phẩm theo danh mục
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
                // Tìm sản phẩm bằng id
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }

                var auctionsForProduct = await _auctionRepository.GetAuctionsByProductIdAsync(product.IProductId);
                _context.TblAuctions.RemoveRange(auctionsForProduct);

                var reviewsForProduct = await _reviewRepository.GetReviewsByProductIdAsync(product.IProductId);
                _context.TblReviews.RemoveRange(reviewsForProduct);

                // Xóa sản phẩm
                _context.TblProducts.Remove(product);

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công
                return Json(new { success = true, message = "Sản phẩm và các bản ghi liên quan đã được xóa thành công." });
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine("Lỗi khi xóa sản phẩm: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                // Trả lại lỗi cho người dùng
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa sản phẩm. " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] TblProduct product)
        {
            try
            {
                if (product == null)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
                }

                await _productRepository.AddAsync(product);
                return Json(new { success = true, message = "Sản phẩm đã được thêm thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditProduct(int id, [FromBody] TblProduct updatedProduct)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }

                product.SProductName = updatedProduct.SProductName;
                product.SDescription = updatedProduct.SDescription;
                product.DStartingPrice = updatedProduct.DStartingPrice;
                product.SImageUrl = updatedProduct.SImageUrl; // Cập nhật thêm các thuộc tính khác nếu cần

                await _productRepository.UpdateAsync(product);
                return Json(new { success = true, message = "Sản phẩm đã được cập nhật." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
