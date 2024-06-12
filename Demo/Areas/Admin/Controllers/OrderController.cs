using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var items = _db.Orders.OrderByDescending(x => x.OrderID).ToList();

            foreach (var item in items)
            {
                item.OrderDate = item.OrderDate ?? DateTime.MinValue;
                item.ShipDate = item.ShipDate ?? DateTime.MinValue;
                item.PaymentDate = item.PaymentDate ?? DateTime.MinValue;
            }

            int pageNumber = page ?? 1;
            int pageSize = 10000;
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
                DateTime dateTime = DateTime.Now;
                item.ShipDate = dateTime.AddMinutes(45);
                _db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "UnSuccess", Success = false });
        }
        public async Task<IActionResult> ViewOrder(int id)
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
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            Console.WriteLine("CancelOrder method called with orderId: " + orderId);

            if (orderId <= 0)
            {
                return BadRequest("Số đơn hàng không hợp lệ.");
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var order = await _db.Orders.FindAsync(orderId);
                    if (order == null)
                    {
                        return NotFound("Không tìm thấy đơn hàng.");
                    }

                    _db.Orders.Remove(order);

                    var orderDetails = await _db.OrderDetails.Where(od => od.OrderID == orderId).ToListAsync();
                    _db.OrderDetails.RemoveRange(orderDetails);

                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    return Ok("Đơn hàng đã được hủy thành công.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"Lỗi: {ex.Message}");
                }
            }
        }





    }
}