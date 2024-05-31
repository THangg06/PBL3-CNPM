using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/HomeAdmin")]
    public class HomeAdminController : Controller
    {
        private readonly Web01Context _db;
        public HomeAdminController(Web01Context db)
        {
            _db = db;
        }

        [Route("")]
        [Route("index")]
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


    }
}