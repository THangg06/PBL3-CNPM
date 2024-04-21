using Demo.Data;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class AccountsController : Controller
    {
        private readonly Web01Context _context;
        public AccountsController(Web01Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
