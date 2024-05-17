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

namespace Demo.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly Web01Context _context;
        //public INotyfService _notyfService {  get; }
        private readonly IMapper _mapper;

        public AccountsController(Web01Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            customer.CreateDate = DateTime.Now;

            // Thêm khách hàng mới vào cơ sở dữ liệu
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login");
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Login.html", Name = "Login")]

        //public async Task<IActionResult> Login(LoginViewModel model, string returnURL = null)
        //{
        //    try
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            bool isEmail = MyUtil.IsValidEmail(model.Email);
        //        if (!isEmail) {
        //                return View(model);
        //                    }
        //            var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == model.Email);
        //        if(khachhang == null)
        //            {
        //                return RedirectToAction("DangKyTaiKhoan");
        //            }
        //            string pass = (model.Password + khachhang.Salt.Trim()).ToMD5();
        //            if(khachhang.Password != pass)
        //            {
        //                //_notyfService.Success("Thông tin đăng nhập chưa chính xác");
        //                return View(model);
        //            }
        //            if(khachhang.Active ==false)

        //            {
        //                return RedirectToAction("ThongBao", "Accounts");
        //            } 

        //            HttpContext.Session.SetString("CustomerId",khachhang.CustomerId.ToString());
        //            var taikhoanID = HttpContext.Session.GetString("CustomerId");
        //            var claims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name, khachhang.FullName),
        //                new Claim("CustomerId", khachhang.CustomerId.ToString())
        //            };

        //            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
        //            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        //            //_notyfService.Success("Đăng nhập thành công");
        //            if (returnURL != null)
        //            {
        //                return Redirect(returnURL);
        //            }
        //            else
        //            {
        //                return RedirectToAction("Dashboard", "Accounts");
        //            }
        //        }

        //    }
        //    catch
        //    {
        //        return RedirectToAction("DangKyTaiKhoan");
        //    }
        //    return View(model);
        //}
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Login.html", Name = "Login")]
        //public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        //{
        //    ViewBag.ReturnUrl = ReturnUrl;

        //    // Kiểm tra xem ModelState có hợp lệ không
        //    if (ModelState.IsValid)
        //    {
        //        // Tìm khách hàng dựa trên email
        //        var khachHang = _context.Customers.SingleOrDefault(kh => kh.Email == model.Email);

        //        // Nếu không tìm thấy khách hàng, thêm thông báo lỗi
        //        if (khachHang == null)
        //        {
        //            ModelState.AddModelError("loi", "Không có khách hàng này.");
        //        }
        //        else
        //        {
        //            // Kiểm tra trạng thái tài khoản của khách hàng
        //            if (khachHang.Active==false)
        //            {
        //                ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
        //            }
        //            else
        //            {
        //                // Mã hóa mật khẩu và so sánh với mật khẩu đã nhập
        //                string pass = (model.Password + khachHang.Salt.Trim()).ToMD5();
        //                if (khachHang.Password != pass)
        //                {
        //                    ModelState.AddModelError("loi", "Sai thông tin đăng nhập.");
        //                }
        //                else
        //                {
        //                    // Thiết lập yêu cầu xác thực và phiên đăng nhập
        //                    var claims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Email, khachHang.Email),
        //                new Claim(ClaimTypes.Name, khachHang.FullName),
        //                //new Claim("CustomerId", khachHang.CustomerId),
        //                new Claim(MySetting.CLAIM_CUSTOMERID, khachHang.CustomerId),
        //                new Claim(ClaimTypes.Role, "Customer") // Role động
        //            };

        //                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        //                    await HttpContext.SignInAsync(claimsPrincipal);

        //                    // Kiểm tra ReturnUrl và chuyển hướng dựa trên URL hợp lệ
        //                    if (Url.IsLocalUrl(ReturnUrl))
        //                    {
        //                        return Redirect(ReturnUrl);
        //                    }
        //                    else
        //                    {
        //                        return RedirectToAction("Index", "Home");
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Nếu có lỗi trong ModelState, trả về trang đăng nhập với thông báo lỗi
        //    return View();
        //}
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
   // new Claim("CustomerId", userCustomer.CustomerId),
    new Claim(MySetting.CLAIM_CUSTOMERID, userCustomer.CustomerId),
    new Claim(ClaimTypes.Role, userRole?.RoleName ?? "Customer")
};

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Đăng nhập vào phiên làm việc
                await HttpContext.SignInAsync(claimsPrincipal);

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
    new Claim(ClaimTypes.Role, userRole?.RoleName ?? "Admin")
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

        [HttpGet]
        [Route("logout", Name = "Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [Route("profile", Name = "Profile")]
        public IActionResult Profile()
        {
            return View();
        }


        
        // [Route("ChangePassword", Name = "Changepassword")]
        // public IActionResult ChangePassword()
        // {
        //     return View();
        //     //HttpContext.SignOutAsync();
        //     //HttpContext.Session.Remove("CustomerId");
        //   //  return RedirectToAction("Index", "Home");
        // }

        // [HttpPost("ChangePassword", Name = "Changepassword")]
        //// [Route("ChangePassword", Name = "Changepassword")]
        // public IActionResult ChangePassword(ChangePasswordCM model)
        // {
        //     if(ModelState.IsValid)
        //     {

        //     }
        //     return View(model);
        //     //HttpContext.SignOutAsync();
        //     //HttpContext.Session.Remove("CustomerId");
        //     //  return RedirectToAction("Index", "Home");
        // }

    }
}
