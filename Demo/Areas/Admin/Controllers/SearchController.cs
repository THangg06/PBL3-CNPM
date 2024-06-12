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
            List<Product> ls = new List<Product>();

            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }

            ls = _context.Products.AsNoTracking()
                .Include(a => a.Cat)
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
                .Take(10)
                .ToList();

            return PartialView("ListProductsSearchPartial", ls);
        }
        [HttpPost]
        public IActionResult FindProductCategory(string keyword)
        {
            List<Category> ls = new List<Category>();

            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            ls = _context.Categories.AsNoTracking()
                .Include(a => a.CatId)
                .Where(x => x.CatName.Contains(keyword))
                .OrderByDescending(x => x.CatName)
                .Take(10)
                .ToList();

            return PartialView("ListProductCategoriesSearchPartial", ls);
        }
        [HttpPost]
        public IActionResult FindCustomer(string keyword)
        {
            List<Customer> ls = new List<Customer>();

            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListCustomersSearchPartial", null);
            }

            ls = _context.Customers.AsNoTracking()
      .Where(x => x.FullName.Contains(keyword))
      .OrderByDescending(x => x.FullName)
      .Take(10)
      .ToList();

            return PartialView("ListCustomersSearchPartial", ls);
        }

    }
}