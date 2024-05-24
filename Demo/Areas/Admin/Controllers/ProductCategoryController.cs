using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly Web01Context _context;
        public ProductCategoryController(Web01Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var items = _context.Categories;

            return View(items);
        }

        public ActionResult Add()
        {
            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if(ModelState.IsValid)
            {
                model.Published = true;
                model.ParentId = 1;
                model.Levels = 1;
                model.Ordering = 1;
                _context.Categories.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }
    }
}
