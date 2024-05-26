using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;
using Demo.Models;
using Demo.Data;

namespace Demo.Controllers
{
    public class NewsController : Controller
    {
        private readonly Web01Context _context;
        //private readonly INotyfService _notyfService; // Sử dụng interface thay vì lớp cụ thể
        public NewsController(Web01Context context) // Sửa đổi ở đây
        {
            _context = context;
            //_notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(News news)
        {
            if (ModelState.IsValid)
            {
                news.DatePosted = DateTime.Now;
                news.Active = 0;
                _context.Add(news);
                await _context.SaveChangesAsync();
                //_notyfService.Success("Gửi liên hệ thành công");
                TempData["SuccessMessage"] = "Tạo tin tức thành công";
                return RedirectToAction("Add");
            }

            return View(news);
        }
    }
}