using Demo.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

using AspNetCoreHero.ToastNotification.Abstractions;

using Demo.Helper;
using Microsoft.AspNetCore.Authorization;


namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        public INotyfService _notyfService { get; }
        private readonly Web01Context _context;

        public AdminProductsController(Web01Context context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminProducts

        public IActionResult Index(int page = 1, string CatID = "")
        {
            var pageNumber = page;
            var pageSize = 30;
            IQueryable<Product> query = _context.Products
      .AsNoTracking()
      .Include(x => x.Cat);

        //    IQueryable<Product> query = _context.Products.AsNoTracking().Include(x => x.Cat);

            // Sử dụng so sánh với CatID
            if (!string.IsNullOrEmpty(CatID))
            {
                query = query.Where(x => x.CatId == CatID);
            }

            var models = new PagedList<Product>(query.Select(p => new Product
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CatName = p.Cat != null ? p.Cat.CatName : "",
                ShortDesc = p.ShortDesc,
                Description = p.Description,
                CatId = p.CatId,
                Price = p.Price,
                Unit = p.Unit,
                Thumb = p.Thumb,
                Video = p.Video,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                BestSellers = p.BestSellers ? p.BestSellers ? true : false : false,
                HomeFlag = p.HomeFlag ? p.HomeFlag ? true : false : false,
                Active = p.Active ? p.Active ? true : false : false,
                Tags = p.Tags,
                Title = p.Title,
                Alias = p.Alias,
                MetaDesc = p.MetaDesc,
                MetaKey = p.MetaKey
            }), pageNumber, pageSize);

            ViewBag.CurrentCatID = CatID;
            ViewBag.CurrentPage = pageNumber;
          //  ViewBag.Danhmuc = _context.Categories.Select(c => new { CatId = c.CatId, CatName = c.CatName }).ToList();

            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName", CatID);

            return View(models);
        }
        public IActionResult Filtter(string CatID = "")
        {
            // Xây dựng URL dựa trên CatID
            var url = string.IsNullOrEmpty(CatID) || CatID == "0" ? "/Admin/AdminProducts" : $"/Admin/AdminProducts?CatID={CatID}";

            return Json(new { status = "success", redirectUrl = url });
        }


        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName");

            // In ra dữ liệu của ViewData["Danhmuc"]
            var danhmuc = ViewData["Danhmuc"] as SelectList;
            if (danhmuc != null)
            {
                Console.WriteLine("Dữ liệu của ViewData['Danhmuc']:");
                foreach (var item in danhmuc)
                {
                    Console.WriteLine($"CatId: {item.Value}, CatName: {item.Text}");
                }
            }

            Console.WriteLine("Hi");

            return View();
        }


     
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("ProductId,ProductName,CatName,ShortDesc,Description,CatId,Price,Unit,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
           
            //ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            //string selectedCatId = Request.Form["CatId"];
            //var selectedCat = _context.Categories.FirstOrDefault(c => c.CatId == selectedCatId);
            //if (selectedCat != null)
            //{
            //    product.CatName = selectedCat.CatName;
            //}

            //var danhmuc = ViewData["Danhmuc"] as SelectList;
            //if (danhmuc != null)
            //{
            //    Console.WriteLine("Dữ liệu của ViewData['Danhmuc']:");
            //    foreach (var item in danhmuc)
            //    {
            //        Console.WriteLine($"CatId: {item.Value}, CatName: {item.Text}");
            //    }
            //}
            ////string selectedCatId = Request.Form["CatId"];

            ////product.CatId = selectedCatId;
            //foreach (var modelState in ModelState.Values)
            //{
            //    foreach (var error in modelState.Errors)
            //    {
            //        Console.WriteLine($"Error: {error.ErrorMessage}");
            //    }
            //}
            //Console.WriteLine("Hieee");
            if (ModelState.IsValid)
            {
                product.ProductName = MyUtil.ToTitleCase(product.ProductName);
             //   product.CatName = MyUtil.ToTitleCase(product.CatName);
                Console.WriteLine("Hie");
                // Xử lý tải lên tệp hình ảnh
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = MyUtil.SEOUrl(product.ProductName) + extension;

                    product.Thumb = await MyUtil.UploadFile(fThumb, @"loadh", image.ToLower());



                }

                if (string.IsNullOrEmpty(product.Thumb))
                {
                    product.Thumb = "default.jpg";
                }

                // Thiết lập các trường còn lại
                product.Alias = MyUtil.SEOUrl(product.ProductName);
                product.DateCreated = DateTime.Now;
                product.DateModified = DateTime.Now;

                // Thêm sản phẩm vào cơ sở dữ liệu
                _context.Add(product);
                _notyfService.Success("Tạo mới thành công");
                await _context.SaveChangesAsync();

                // Chuyển hướng về danh sách
                return RedirectToAction(nameof(Index));
            }



            // Nếu ModelState không hợp lệ, trả về View cùng với dữ liệu hiện có
            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Unit,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductName = MyUtil.ToTitleCase(product.ProductName);
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = MyUtil.SEOUrl(product.ProductName) + extension;
                        product.Thumb = await MyUtil.UploadFile(fThumb, @"products", image.ToLower());

                    }
                    if (string.IsNullOrEmpty(product.Thumb)) product.Thumb = "default.jpg";
                    product.Alias = MyUtil.SEOUrl(product.ProductName);
                    product.DateModified = DateTime.Now;


                    _notyfService.Success("Cập nhật thành công");
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        _notyfService.Success("Có lỗi xảy ra");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Danhmuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}