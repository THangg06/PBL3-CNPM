using AutoMapper;
using Demo.Data;
using Demo.Extension;
using Demo.Helper;
using Demo.ModelViews;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public IActionResult ValidatePhone(string Phone){
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
                if (khachhang == null)
                {
                    return Json(data: "Số điện thoại : " + Phone + " Đã được sử dụng");
                    
                }
                return Json(data: true);
            } catch
            {
                return Json(data: true);
            }    
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking() .SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang == null)
                {
                    return Json(data: "Số điện thoại : " + Email + " Đã được sử dụng");

                }
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [Route("TaiKhoancuatoi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
               var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x=>x.CustomerId==Convert.ToInt32(taikhoanID));
                if(khachhang!=null)
                {
                    return View(khachhang);
                }    
            }
            return RedirectToAction("Login");
        }
        
        [AllowAnonymous]
        [Route("Login.html", Name = "Login")]
        public IActionResult Login()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Accounts");
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Register.html", Name = "Register")]
        public IActionResult DangKyTaiKhoan()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register.html", Name = "Register")]

        public async Task<IActionResult> DangKyTaiKhoan(RegisterViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    string salt = MyUtil.GenerateRandomKey();
                    Customer khachhang = new Customer
                    {
                        FullName = model.FullName,
                        Email = model.Email.Trim().ToLower(),
                        Phone = model.Phone.Trim().ToLower(),
                        Birthday = model.Date,
                        Password = (model.Password + salt.Trim()).ToMD5(),
                        Active = true,
                        Salt = salt,
                        CreateDate= DateTime.Now

                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.FullName),
                            new Claim("CustomerId", khachhang.CustomerId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch 
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");
                    };

                }    
                else
                {
                    return View(model);
                }
            } catch
            {
                return View(model);
            }
        
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Login.html", Name = "Login")]

        public async Task<IActionResult> Login(LoginViewModel model, string returnURL = null)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    bool isEmail = MyUtil.IsValidEmail(model.Email);
                if (!isEmail) {
                        return View(model);
                            }
                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == model.Email);
                if(khachhang == null)
                    {
                        return RedirectToAction("DangKyTaiKhoan");
                    }
                    string pass = (model.Password + khachhang.Salt.Trim()).ToMD5();
                    if(khachhang.Password != pass)
                    {
                        //_notyfService.Success("Thông tin đăng nhập chưa chính xác");
                        return View(model);
                    }
                    if(khachhang.Active ==false)

                    {
                        return RedirectToAction("ThongBao", "Accounts");
                    } 
                    
                    HttpContext.Session.SetString("CustomerId",khachhang.CustomerId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim("CustomerId", khachhang.CustomerId.ToString())
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    //_notyfService.Success("Đăng nhập thành công");
                    return RedirectToAction("Checkout");
                }
              
            }
            catch
            {
                return View("DangKyTaiKhoan", "Accounts");
            }
            return View(model);
        }


      
    }
}
