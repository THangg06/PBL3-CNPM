using Demo.Data;
using Demo.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Web01Context db;

        public HangHoaController(Web01Context context)
        {
            db = context;
        }
       
        public IActionResult Index(string? loai)
        {

            var hanghoa = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(loai))
            {
                hanghoa = hanghoa.Where(p => p.CatId == loai);
            }

            var result = hanghoa.Select(p => new HangHoaVM
            {
                MaHH = p.ProductId,
                TenHH = p.ProductName,
                Hinh = p.Thumb ?? "",
                Dongia = p.Price,
                MoTaNgan = p.Unit ?? "",
                TenLoai = p.Cat != null ? p.Cat.CatName : ""
            });


            return View(result);
        }

        public IActionResult Detail(string id)
        {
            var data = db.Products
        .Include(p => p.Cat)
        .SingleOrDefault(p => p.ProductId == id);

            Console.WriteLine($"ID of the product: {id}");
            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return RedirectToAction("PageNotFound", "Home");
            }

            var result = new ChiTietHangHoaVM
            {
                MaHH = data.ProductId,
                TenHH = data.ProductName,
                Dongia = data.Price,
                Hinh = data.Thumb ?? "",
                ChiTiet = data.Description ?? string.Empty ,
                MoTaNgan = data.Unit ?? "",
                TenLoai = data.Cat != null ? data.Cat.CatName : "",
         
                SoLuongTon =10, 
                DiemDanhGia = 5 
            };

            return View(result);
        }
        private int GetSoLuongTonFromAnotherSource(string productId)
        {
          
            return 20; 
        }

        private int GetDiemDanhGiaFromAnotherSource(string productId)
        {
           
            return 20; 
        }
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
     
            List<Product> ls = new List<Product>();

            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }

            ls = db.Products.AsNoTracking()
                .Include(a => a.Cat)
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
           
                .ToList();

            return PartialView("ListProductsSearchPartial", ls);
        }


    }
}
