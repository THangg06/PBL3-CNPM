using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly Web01Context _context;
        public SearchController(Web01Context context)
        {
            _context = context;
        }
    
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            // Tạo danh sách sản phẩm rỗng
            List<Product> ls = new List<Product>();

            // Kiểm tra xem từ khóa có hợp lệ không
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }

            // Truy vấn cơ sở dữ liệu
            ls = _context.Products.AsNoTracking()
                .Include(a => a.Cat)
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
                .Take(10)
                .ToList();

            // Trả về kết quả cho PartialView
            return PartialView("ListProductsSearchPartial", ls);
        }
        [HttpPost]
        public IActionResult FindProductCategory(string keyword)
        {
            // Tạo danh sách sản phẩm rỗng
            List<Category> ls = new List<Category>();

            // Kiểm tra xem từ khóa có hợp lệ không
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }

            // Truy vấn cơ sở dữ liệu
            ls = _context.Categories.AsNoTracking()
                .Include(a => a.CatId)
                .Where(x => x.CatName.Contains(keyword))
                .OrderByDescending(x => x.CatName)
                .Take(10)
                .ToList();

            // Trả về kết quả cho PartialView
            return PartialView("ListProductCategoriesSearchPartial", ls);
        }
        [HttpPost]
        public IActionResult FindCustomer(string keyword)
        {
            // Tạo danh sách sản phẩm rỗng
            List<Customer> ls = new List<Customer>();

            // Kiểm tra xem từ khóa có hợp lệ không
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListCustomersSearchPartial", null);
            }

            ls = _context.Customers.AsNoTracking()
      .Where(x => x.FullName.Contains(keyword))
      .OrderByDescending(x => x.FullName)
      .Take(10)
      .ToList();


            // Trả về kết quả cho PartialView
            return PartialView("ListCustomersSearchPartial", ls);
        }

    }
}