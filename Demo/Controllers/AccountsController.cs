using AutoMapper;
using Demo.Data;
using Demo.Extension;
using Demo.Helper;
using Demo.ModelViews;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Demo.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly Web01Context _context;
        //  public INotyfService _notyfService {  get; }
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsController> _logger;
        private readonly INotyfService _notyfService;

        public AccountsController(Web01Context context, IMapper mapper, INotyfService notyfService)
        {
            _context = context;
            _mapper = mapper;
            _notyfService = notyfService;
            //     _notyfService = notyfService;
            //_notyfService = notyfService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string phone)
        {
            try
            {
                // Kiểm tra xem số điện thoại đã tồn tại hay chưa
                var customer = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone == phone);

                if (customer != null)
                {
                    // Nếu số điện thoại đã tồn tại, trả về thông báo lỗi
                    return Json(new { success = false, message = $"Số điện thoại {phone} đã được sử dụng." });
                }

                // Nếu số điện thoại chưa tồn tại, trả về true
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có và trả về thông báo lỗi
                return Json(new { success = false, message = $"Có lỗi xảy ra khi kiểm tra số điện thoại: {ex.Message}" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string email)
        {

            try
            {
                // Kiểm tra xem email đã tồn tại hay chưa
                var customer = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email == email);

                if (customer != null)
                {
                    // Nếu email đã tồn tại, trả về thông báo lỗi
                    return Json(new { success = false, message = $"Email {email} đã được sử dụng." });
                }

                // Nếu email chưa tồn tại, trả về true
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có và trả về thông báo lỗi
                return Json(new { success = false, message = $"Có lỗi xảy ra khi kiểm tra email: {ex.Message}" });
            }
        }

        [Route("TaiKhoancuatoi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            // Lấy CustomerId từ session
            var taikhoanID = HttpContext.Session.GetString("CustomerId");

            // Kiểm tra nếu CustomerId tồn tại và không null
            if (!string.IsNullOrEmpty(taikhoanID))
            {
                // Tìm khách hàng trong cơ sở dữ liệu theo CustomerId
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == taikhoanID);

                // Nếu tìm thấy khách hàng, trả về view với thông tin khách hàng
                if (khachhang != null)
                {
                    return View(khachhang);
                }
            }

            // Nếu không tìm thấy khách hàng hoặc CustomerId không hợp lệ, chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login");
        }


        [AllowAnonymous]
        [Route("Login.html", Name = "Login")]
        public IActionResult Login()
        {
            // Lấy CustomerId từ session
            var taikhoanID = HttpContext.Session.GetString("CustomerId");

            // Nếu CustomerId tồn tại, chuyển hướng đến trang Dashboard
            if (!string.IsNullOrEmpty(taikhoanID))
            {
                return RedirectToAction("Dashboard");
            }

            // Trả về trang đăng nhập nếu chưa đăng nhập
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Register.html", Name = "Register")]
        public IActionResult DangKyTaiKhoan()
        {
            return View();
        }


        public bool IsEmailInUse(string email)
        {
            return _context.Customers.AsNoTracking().Any(c => c.Email == email);
        }

        public bool IsPhoneInUse(string phone)
        {
            return _context.Customers.AsNoTracking().Any(c => c.Phone == phone);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register.html", Name = "Register")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra email và số điện thoại đã sử dụng hay chưa
            if (IsEmailInUse(model.Email))
            {
                ModelState.AddModelError(string.Empty, "Email đã được sử dụng.");
                return View(model);
            }
            if (IsPhoneInUse(model.Phone))
            {
                ModelState.AddModelError(string.Empty, "Số điện thoại đã được sử dụng.");
                return View(model);
            }

            // Tạo đối tượng khách hàng mới và thêm vào cơ sở dữ liệu
            var customer = _mapper.Map<Customer>(model);
            string salt = MyUtil.GenerateRandomKey();
            customer.Salt = salt;
            customer.Password = (model.Password + salt.Trim()).ToMD5();
            customer.Active = true;
            customer.RoleId = 2;
            customer.CreateDate = DateTime.Now;
            customer.Avatar = "images.png";

            // Thêm khách hàng mới vào cơ sở dữ liệu
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            //_notyfService.Success("Đăng ký thành công");
            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            // Kiểm tra xem ModelState có hợp lệ không
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra khách hàng (Customer) dựa trên email
            Customer? userCustomer = _context.Customers.SingleOrDefault(kh => kh.Email == model.Email);

            if (userCustomer != null)
            {
                // Xử lý đăng nhập cho khách hàng
                if (!userCustomer.Active.HasValue || !userCustomer.Active.Value)
                {
                    ModelState.AddModelError("loi", "Tài khoản đã bị khóa.");
                    return View(model);
                }

                // Mã hóa mật khẩu và so sánh với mật khẩu đã nhập
                string hashedPassword = (model.Password + userCustomer.Salt?.Trim()).ToMD5();
                if (userCustomer.Password != hashedPassword)
                {
                    ModelState.AddModelError("loi", "Sai thông tin đăng nhập.");
                    return View(model);
                }

                // Lấy vai trò của khách hàng
                Role? userRole = _context.Roles.SingleOrDefault(role => role.RoleId == userCustomer.RoleId);


                // Tạo claims cho phiên đăng nhập
                var claims = new List<Claim>
{

    new Claim(ClaimTypes.Email, userCustomer.Email),
    new Claim(ClaimTypes.Name, userCustomer.FullName ?? ""),

    //new Claim(ClaimTypes.Name, userCustomer.FullName ?? ""),
   new Claim("CustomerId", userCustomer.CustomerId),
    new Claim(MySetting.CLAIM_CUSTOMERID, userCustomer.CustomerId),
    new Claim(ClaimTypes.Role, userRole?.RoleName ?? "Customer"),
    new Claim("PhoneNumber", userCustomer.Phone ?? ""),
    new Claim("DateOfBirth", userCustomer.Birthday?.ToString("yyyy-MM-dd") ?? ""),
    new Claim("Address", userCustomer.Address ?? ""),
     new Claim("Avatar", userCustomer.Avatar ?? "")
};

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Đăng nhập vào phiên làm việc
                await HttpContext.SignInAsync(claimsPrincipal);
               
                HttpContext.Session.SetString("CustomerId", userCustomer.CustomerId);
                Console.WriteLine("CustomerId cua ban là: " + userCustomer.CustomerId);
                // Chuyển hướng dựa trên vai trò
                if (Url.IsLocalUrl(ReturnUrl))
                {
                   
                    return Redirect(ReturnUrl);
                }
                else
                {
                  
                    return RedirectToAction("Index", "Home");
                }
            }

            // Nếu không tìm thấy khách hàng, kiểm tra quản trị viên (Account)
            Account? userAdmin = _context.Accounts.SingleOrDefault(ac => ac.Email == model.Email);

            if (userAdmin != null)
            {
                // Xử lý đăng nhập cho quản trị viên
                if (!userAdmin.Active.HasValue || !userAdmin.Active.Value)
                {
                    ModelState.AddModelError("loi", "Tài khoản đã bị khóa.");
                    return View(model);
                }

                // Mã hóa mật khẩu và so sánh với mật khẩu đã nhập
                string hashedPassword = (model.Password + userAdmin.Salt?.Trim()).ToMD5();
                if (userAdmin.Password != hashedPassword)
                {
                    ModelState.AddModelError("loi", "Sai thông tin đăng nhập.");
                    return View(model);
                }

                // Lấy vai trò của quản trị viên
                Role? userRole = _context.Roles.SingleOrDefault(role => role.RoleId == userAdmin.RoleId);

                // Tạo claims cho phiên đăng nhập
                var claims = new List<Claim>
{
    new Claim(ClaimTypes.Email, userAdmin.Email),
 new Claim(ClaimTypes.Name, userAdmin.FullName ?? ""),
 new Claim("AccountId", userAdmin.AccountId ?? ""),
 new Claim("Avatar", userAdmin.Avatar ?? ""),
 new Claim(ClaimTypes.Role, userRole?.RoleName ?? "Admin"),
};

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Đăng nhập vào phiên làm việc
                await HttpContext.SignInAsync(claimsPrincipal);

                // Chuyển hướng dựa trên vai trò
                if (claimsPrincipal.IsInRole("Admin"))
                {
                    // Chuyển hướng đến trang HomeAdmin trong khu vực Admin
                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // Nếu không tìm thấy người dùng
            ModelState.AddModelError("loi", "Không tìm thấy người dùng.");
            return View(model);
        }



      

        [AllowAnonymous]
        public IActionResult ChangePassword()
        {
            // Lấy CustomerId từ session
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            Console.WriteLine("CustomerId cuâ abạn là: " + taikhoanID);
            // Nếu CustomerId tồn tại, chuyển hướng đến trang Dashboard
            //if (!string.IsNullOrEmpty(taikhoanID))
            //{
            //    return RedirectToAction("DangKyTaiKhoan");
            //}

            // Trả về trang đăng nhập nếu chưa đăng nhập
            return View();
        }


        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        //[Route("ChangePassword.html", Name = "ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCM model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                Console.WriteLine("ID lấy từ session: " + taikhoanID);

                if (string.IsNullOrEmpty(taikhoanID))
                {
                    Console.WriteLine("ID null hoặc rỗng");
                    return RedirectToAction("DangKyTaiKhoan", "Accounts");
                }
              
                if (!ModelState.IsValid)
                {
                  

                    Console.WriteLine("Model state không hợp lệ");
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"Lỗi: {error.ErrorMessage}");
                        }
                    }
                    return View(model);
                }

                Console.WriteLine("Model state hợp lệ, tìm tài khoản");

                var taikhoan = await _context.Customers.FindAsync(taikhoanID);
               
                Console.WriteLine("Tài khoản là: " + (taikhoan != null ? taikhoan.CustomerId : "null"));
            
                if (taikhoan == null)
                {
                   
                    return RedirectToAction("DangKyTaiKhoan", "Accounts");
                }

                // Verifying the current password
                var pass = (model.CurrentPassword.Trim() + taikhoan.Salt.Trim()).ToMD5();
            

                if (pass != taikhoan.Password)
                {
                    TempData["ErrorMessage"] = "Bạn đã nhập sai mật khẩu.";
                  
                    ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                    return View(model);
                }

                // Updating with the new password
                string passnew = (model.NewPassword.Trim() + taikhoan.Salt.Trim()).ToMD5();
                taikhoan.Password = passnew;
                _context.Customers.Update(taikhoan);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
                TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                Console.WriteLine("Lỗi: " + ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while changing the password. Please try again later.");
                return View(model);
            }
        }



        [HttpGet]
        [Route("logout", Name = "Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }

        //[Authorize]
        //[Route("profile", Name = "Profile")]
        //public IActionResult Profile()
        //{
        //    var customerId = HttpContext.Session.GetString("CustomerId");
        //    if (string.IsNullOrEmpty(customerId))
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var existingCustomer = _context.Customers.Find(customerId);
        //    if (existingCustomer == null)
        //    {
        //        return NotFound();
        //    }

        //    // Trích xuất thông báo từ TempData (nếu có)
        //    ViewBag.Message = TempData["Message"];

        //    return View(existingCustomer);
        //}
        [Authorize]
        [Route("profile", Name = "Profile")]
        public IActionResult Profile()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("Login");
            }

            var existingCustomer = _context.Customers.Find(customerId);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Trích xuất thông báo từ TempData (nếu có)
            ViewBag.UpdateMessage = TempData["UpdateMessage"];

            return View(existingCustomer);
        }

        [HttpGet]
        [Route("EditProfile")]
        public IActionResult EditProfile()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("Login");
            }

            var existingCustomer = _context.Customers.Find(customerId);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            return View(existingCustomer);
        }

        [HttpPost]
        [Route("EditProfile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            try
            {
                var existingCustomer = await _context.Customers.FindAsync(customer.CustomerId);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                // Cập nhật các trường không nên được chỉnh sửa
                customer.Password = existingCustomer.Password;
                customer.Salt = existingCustomer.Salt;
                customer.Active = existingCustomer.Active;
                customer.CreateDate = existingCustomer.CreateDate;
                customer.Avatar = existingCustomer.Avatar;

                // Cập nhật thông tin khách hàng
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
                await _context.SaveChangesAsync();
                //_notyfService.Success("Chỉnh sửa thành công");
                // Chuyển hướng đến trang profile với dữ liệu đã được cập nhật
                TempData["Message"] = "Thông tin khách hàng đã được cập nhật thành công.";
                return RedirectToAction("Profile");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Xử lý ngoại lệ và hiển thị thông báo lỗi cho người dùng
                Console.WriteLine(ex);
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin khách hàng.");
                return View(customer);
            }
        }


        [HttpPost]
        [Route("UpdateProfile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", customer);
            }

            try
            {
                var existingCustomer = await _context.Customers.FindAsync(customer.CustomerId);
                if (existingCustomer == null)
                {
                    return NotFound();
                }
                //  Cập nhật thông tin từ form
                existingCustomer.FullName = customer.FullName;
                existingCustomer.Email = customer.Email;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Birthday = customer.Birthday;
                existingCustomer.Address = customer.Address;

                _context.Update(existingCustomer);
                await _context.SaveChangesAsync();
                _notyfService.Success("Chỉnh sửa thành công");
                TempData["UpdateMessage"] = "Thông tin của bạn đã được cập nhật thành công.";
                return View(existingCustomer);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin khách hàng.");
                return View("Profile", customer);
            }


        }

        private bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
        [HttpGet]
        [Route("History")]
        public async Task<IActionResult> PurchaseHistory()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("Login");
            }

            var existingCustomer = await _context.Customers.FindAsync(customerId);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
      .Include(o => o.OrderDetails)
          .ThenInclude(od => od.Product) 
      .Where(o => o.CustomerId == customerId)
      .ToListAsync();

            return View(orders);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ForgotPassword", Name = "ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword", Name = "ForgotPasswordPost")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = _context.Customers.SingleOrDefault(c => c.Email == model.Email);
            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, "Email not found.");
                return View(model);
            }

            // Redirect to a view for entering a new password, passing the email as a query parameter
            return RedirectToAction("ResetPassword", new { email = model.Email });
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = _context.Customers.SingleOrDefault(c => c.Email == model.Email);
            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, "Email not found.");
                return View(model);
            }

            // Update the password for the user with the provided email
            //customer.Password = model.NewPassword; // Update this with your password hashing logic
            //await _context.SaveChangesAsync();
            string passnew = (model.NewPassword.Trim() + customer.Salt.Trim()).ToMD5();
            customer.Password = passnew;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();


            // Optionally, redirect the user to a login page or a page indicating successful password reset
            return RedirectToAction("Login", "Accounts");
        }


    }
}
