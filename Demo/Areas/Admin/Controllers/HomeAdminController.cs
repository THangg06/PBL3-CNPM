using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/HomeAdmin")]
    public class HomeAdminController : Controller
    {
        private readonly Web01Context db;
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.Danhmuc = new SelectList(db.Categories.ToList(), "CatId", "CatName");
            return View();
        }
        [Route("ThemSanPhamMoi")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ThemSanPhamMoi(Category SanPham) {
            if(ModelState.IsValid)
            {
                db.Categories.Add(SanPham);
                db.SaveChanges();
                return RedirectToAction("Index", "ProductCategory");
            }
            return View(SanPham);
        }
    }
}
