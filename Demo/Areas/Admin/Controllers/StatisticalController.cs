using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticalController : Controller
    {
        private readonly Web01Context _db;

        public StatisticalController (Web01Context db)
        {
            _db = db;
        }
       
        public IActionResult Index()
        {
           
            return View();
        }

        //public IActionResult RevenueChart()
     //   {
     ////       var revenueByMonth = _db.Orders
     ////.GroupBy(p => DbFunctions.TruncateTime(p.OrderDate).Value.Month)
     ////.Select(g => new { Month = g.Key, TotalRevenue = g.Sum(p => p.TongTien) })
     ////.ToList();

     //       //ViewBag.RevenueData = revenueByMonth;
     //       //return View(revenueByMonth);
     //   }
    }
}
