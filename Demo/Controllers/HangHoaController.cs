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
        //public async Task<IActionResult> Detail(string ID)
        //{
        //    if(ID == null) return RedirectToAction("Index");
        //    var Prid = db.Products.Where(p => p.ProductId  == ID ).FirstOrDefault();
        //    return View(Prid);
        //}

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
                // Here you can retrieve SoLuongTon and DiemDanhGia from another data source if needed
                SoLuongTon =10, // Example method, replace with your logic
                DiemDanhGia = 5 // Example method, replace with your logic
            };

            return View(result);
        }
        private int GetSoLuongTonFromAnotherSource(string productId)
        {
            // Example implementation:
            // Assuming you have another data source or service that provides the quantity in stock for a given product ID
            // You would query that data source here and return the quantity in stock

            // For demonstration purposes, let's return a hardcoded value
            return 20; // Replace this with your actual logic to retrieve the quantity in stock
        }

        private int GetDiemDanhGiaFromAnotherSource(string productId)
        {
            // Example implementation:
            // Assuming you have another data source or service that provides the quantity in stock for a given product ID
            // You would query that data source here and return the quantity in stock

            // For demonstration purposes, let's return a hardcoded value
            return 20; // Replace this with your actual logic to retrieve the quantity in stock
        }
       

    }
}
