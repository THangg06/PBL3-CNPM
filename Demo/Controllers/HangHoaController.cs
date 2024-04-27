using Demo.Data;
using Demo.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Web01Context db;

        public HangHoaController(Web01Context context) {
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
                Dongia = p.Price ?? 0,
                MoTaNgan = p.Unit ?? "",
                TenLoai = p.Cat != null ? p.Cat.CatName : ""
            }); 

            
            return View(result);
        }

    }
}
