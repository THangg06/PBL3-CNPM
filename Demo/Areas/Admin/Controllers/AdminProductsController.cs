using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly Web01Context _context;

        public AdminProductsController(Web01Context context)
        {
            _context = context;
        }

        public IActionResult Index(int? page, string CatID = "")
        {
            var products = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(CatID) && CatID != "0")
            {
                products = products.Where(p => p.CatId == CatID);
            }

            var items = products.OrderByDescending(x => x.ProductId);
            var pageSize = 10;
            var pageIndex = page ?? 1;

            var pagedList = items.ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.CurrentCatID = CatID;

            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName", CatID);
            return View(pagedList);
        }

        public ActionResult Add()
        {
            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Product model)
        {
            if (ModelState.IsValid)
            {
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                _context.Products.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View(model);
        }
    }
}
