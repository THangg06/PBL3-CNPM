using Microsoft.AspNetCore.Mvc;

namespace Demo.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
