using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using PagedList.Core.Mvc;
namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly Web01Context _db;
        public OrderController(Web01Context db)
        {
            _db = db;
        }

        public IActionResult Index(int? page)
        {
            var items = _db.Orders.OrderByDescending(x => x.OrderDate).ToList();

            int pageNumber = page ?? 1;
            int pageSize = 25;
            int totalItems = items.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;

            return View(pagedItems);
        }



        public async Task<IActionResult> View(int id)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = _db.Orders.Find(id);
            if (item != null)
            {
                _db.Orders.Attach(item);
                item.TransactStatusId = trangthai;
                _db.Entry(item).Property(x => x.TransactStatusId).IsModified = true;
                item.ShipDate = DateTime.Now;
                _db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "UnSuccess", Success = false });
        }

    }
}