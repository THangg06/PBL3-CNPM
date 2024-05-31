using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticalController : Controller
    {
        private readonly Web01Context _db;

        public StatisticalController(Web01Context db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult RevenueChart()
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
        public IActionResult Statistical_Chart_Pie()
        {
            var distinctRates = _db.ReviewProduct
                .Select(r => EF.Property<int>(r, "Rate"))
                .Distinct()
                .ToList();

            var rateCounts = new List<int>();

            int countSumRate = (_db.ReviewProduct.Select(p => p.Rate)).Count();

            foreach (var rate in distinctRates)
            {
                int countRate = _db.ReviewProduct
                    .Where(o => EF.Property<int>(o, "Rate") == rate)
                    .Count();

                rateCounts.Add(countRate);
            }

            ViewBag.CountSumRate = countSumRate;
            ViewBag.DistinctRates = distinctRates;
            ViewBag.RateCounts = rateCounts;

            return View();
        }
        public IActionResult Statistical_Chart_Bar()
        {
            var today = DateTime.UtcNow.Date;
            var recentDates = _db.Orders
       .Where(o => EF.Property<DateTime>(o, "OrderDate") >= today.AddDays(-4))
       .OrderByDescending(o => o.OrderDate)
       .Select(o => EF.Property<DateTime>(o, "OrderDate").Date)
       .Distinct()
       .Take(5)
       .ToList();

            var revenueByDate = new List<int>();
            int Total = 0;
            foreach (var date in recentDates)
            {
                int revenue = (_db.Orders
                    .Where(o => EF.Property<DateTime>(o, "OrderDate").Date == date)
                    ).Count();

                revenueByDate.Add(revenue);
                Total++;
            }



            ViewBag.RecentDates = recentDates;
            ViewBag.RevenueByDate = revenueByDate;
            ViewBag.CountsumRevenue = Total;

            return View();
        }

    }
}