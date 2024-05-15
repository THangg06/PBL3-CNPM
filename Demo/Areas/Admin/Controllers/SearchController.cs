<<<<<<< HEAD
﻿using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly Web01Context _context;
        public SearchController (Web01Context context)
        {
            _context = context;
        }
        [HttpPost]
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


    }
}
=======
﻿using Demo.Data;
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


    }
}
>>>>>>> 06235904bb357cf68a24e145336d06811074051c
