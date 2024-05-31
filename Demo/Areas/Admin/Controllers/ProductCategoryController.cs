using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Data;
using Demo.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly Web01Context _context;
        public INotyfService _notyfService { get; }
        public ProductCategoryController(Web01Context context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            var items = _context.Categories;

            return View(items);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        public ActionResult Add()
        {
            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("CatId,CatName,Description,ParentId,Levels,Ordering,Published,Thumb,Title,Alias,MetaDesc,MetaKey,Cover,SchemaMarkup")] Category category, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
           
                if (ModelState.IsValid)
                {
                    category.CatName = MyUtil.ToTitleCase(category.CatName);

                    // Xử lý tải lên tệp hình ảnh
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = MyUtil.SEOUrl(category.CatName) + extension;

                        category.Thumb = await MyUtil.UploadFile(fThumb, @"LoadH", image.ToLower());
                    }


                    // Kiểm tra và đặt Thumb nếu không có
                    if (string.IsNullOrEmpty(category.Thumb))
                    {
                        category.Thumb = "default.jpg";
                    }


                    category.Published = true;
                    category.ParentId = 1;
                    category.Levels = 1;
                    category.Ordering = 1;
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    _notyfService.Success("Tạo mới thành công");
                    return RedirectToAction("Index");
                }

            
                ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName");
                return View(category);
            
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CatId,CatName,Description,ParentId,Levels,Ordering,Published,Thumb,Title,Alias,MetaDesc,MetaKey,Cover,SchemaMarkup")] Category category, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != category.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.CatName = MyUtil.ToTitleCase(category.CatName);

                   
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = MyUtil.SEOUrl(category.CatName) + extension;

                        category.Thumb = await MyUtil.UploadFile(fThumb, @"LoadH", image.ToLower());
                    }


                    // Kiểm tra và đặt Thumb nếu không có
                    if (string.IsNullOrEmpty(category.Thumb))
                    {
                        category.Thumb = "default.jpg";
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }



        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string id)
        {
            return _context.Categories.Any(e => e.CatId == id);
        }
    }
}
