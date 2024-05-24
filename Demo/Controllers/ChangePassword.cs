using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class ChangePassword : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
