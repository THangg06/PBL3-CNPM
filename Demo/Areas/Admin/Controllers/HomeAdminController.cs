using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/HomeAdmin")]
    public class HomeAdminController : Controller
    {
        private readonly Web01Context _db;
        public INotyfService _notyfService { get; }

        public HomeAdminController(Web01Context db, INotyfService notyfService)
        {
            _db = db;
            _notyfService = notyfService;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var today = DateTime.UtcNow.Date;
            var recentDates = _db.Orders
                .Where(o => EF.Property<DateTime>(o, "OrderDate") >= today.AddDays(-4))
                .OrderByDescending(o => o.OrderDate)
                .Select(o => EF.Property<DateTime>(o, "OrderDate").Date)
                .Distinct()
                .Take(5)
                .ToList();
            var revenueByDate = new List<decimal>();
            decimal Total = 0;
            foreach (var date in recentDates)
            {
                decimal revenue = _db.Orders
                    .Where(o => EF.Property<DateTime>(o, "OrderDate").Date == date)
                    .Sum(o => o.TongTien);

                revenueByDate.Add(revenue);
                Total += revenue;
            }

            ViewBag.RecentDates = recentDates;
            ViewBag.RevenueByDate = revenueByDate;
            ViewBag.CountsumRevenue = Total;

            return View();
        }

        [HttpGet]
        [Route("ProfileAdmin")]
        public IActionResult ProfileAdmin()
        {
            var adminid = User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
            if (string.IsNullOrEmpty(adminid))
            {
                return RedirectToAction("Login", "Accounts");
            }

            var existingCustomer = _db.Accounts.Find(adminid);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            return View(existingCustomer);
        }

        [HttpGet]
        [Route("EditAvatar")]
        public IActionResult EditAvatar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login", "Accounts");
            }

            var item = _db.Accounts.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [Route("EditAvatar")]
        public async Task<IActionResult> EditAvatar(string id, IFormFile Avatar)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login", "Accounts");
            }

            var item = _db.Accounts.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            if (Avatar != null)
            {
                string extension = Path.GetExtension(Avatar.FileName);
                string imageName = Path.GetFileNameWithoutExtension(Avatar.FileName) + extension;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/admin", imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await Avatar.CopyToAsync(stream);
                }

                item.Avatar = imageName;
            }

            _db.Update(item);
            await _db.SaveChangesAsync();

            TempData["UpdateMessage"] = "Avatar đã được cập nhật.";
            return RedirectToAction("ProfileAdmin", "HomeAdmin");
        }

        [HttpPost]
        [Route("UpdateProfileAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfileAdmin(Account account)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", account);
            }

            try
            {
                var existingCustomer = await _db.Accounts.FindAsync(account.AccountId);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                existingCustomer.FullName = account.FullName;
                existingCustomer.Email = account.Email;
                existingCustomer.Phone = account.Phone;

                _db.Update(existingCustomer);
                await _db.SaveChangesAsync();
                _notyfService.Success("Chỉnh sửa thành công");
                TempData["UpdateMessage"] = "Thông tin của bạn đã được cập nhật thành công.";
                return View(existingCustomer);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin khách hàng.");
                return View("Profile", account);
            }
        }

        [HttpGet]
        [Route("Inf")]
        public IActionResult Inf()
        {
            var adminId = User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login", "Accounts");
            }

            var item = _db.Accounts.FirstOrDefault(a => a.AccountId == adminId);
            if (item == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Admin/Views/HomeAdmin/Inf.cshtml", item);
        }
    }
}
