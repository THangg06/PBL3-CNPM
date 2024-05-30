using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Demo.Data;
using Demo.ModelViews;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demo.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly Web01Context _db;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(Web01Context db, ILogger<ReviewController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var item = _db.ReviewProduct.ToList();
            return View(item);
        }

        [AllowAnonymous]
        public async Task<ActionResult> _Review(string productId)
        {
            ViewBag.ProductId = productId;
            var item = new ReviewProduct
            {
                ProductId = productId
            };
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
                var userFullName = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
              
                item.Email = userEmail;
                item.FullName = userFullName;

                return PartialView(item);
            }

            return RedirectToAction("Login", "Accounts");
        }
      
        [AllowAnonymous]
        public ActionResult _Load_Review(string productId)
        {
            var items = _db.ReviewProduct.Where(p => p.ProductId == productId).OrderByDescending(p => p.CreatedDate).ToList();
            ViewBag.Count = items.Count;
            return PartialView( items);
        }



        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostReview(ReviewProduct rev)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userCustomerIdClaim = @User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
                    string userCustomerId = userCustomerIdClaim != null ? userCustomerIdClaim.Value : "";

                    rev.UserName = userCustomerId;


                    rev.CreatedDate = DateTime.Now;
                    var userAvatar = @User.Claims.FirstOrDefault(c => c.Type == "Avatar")?.Value;
                    rev.Avatar = userAvatar;
                    await _db.ReviewProduct.AddAsync(rev);
                    await _db.SaveChangesAsync();
                    Console.WriteLine("ProductId: " + rev.ProductId);
                    return RedirectToAction("Index", "HangHoa", new { productId = rev.ProductId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving review");
                    return Json(new { Success = false, Errors = new[] { "An error occurred while saving the review." } });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("ModelState invalid: {@Errors}", errors);
                return Json(new { Success = false, Errors = errors });
            }
        }
    }
}
