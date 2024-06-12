using Demo.Data;
using Demo.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Demo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Demo.Controllers
{
    public class CartController : Controller
    {
        private readonly Web01Context _db;
        //public INotyfService _notyfService { get; }
        public CartController(Web01Context context)
        {
            _db = context;
            //_notyfService = notyfService;
        }

        private List<CartItem> Cart
        {
            get => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
            set => HttpContext.Session.Set(MySetting.CART_KEY, value);
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove(MySetting.CART_KEY);
        }

        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(string id, int quantity = 1)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.MaHh.ToString() == id);

            if (item == null)
            {
                var product = _db.Products.SingleOrDefault(p => p.ProductId == id);

                if (product == null)
                {
                    TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                    return Redirect("/404");
                }

                item = new CartItem
                {
                    MaHh = id,
                    TenHH = product.ProductName,
                    Dongia = product.Price,
                    Hinh = product.Thumb ?? string.Empty,
                    SoLuong = quantity
                };
                cart.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            Cart = cart;
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(string id)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.MaHh.ToString() == id);

            if (item != null)
            {
                cart.Remove(item);
                Cart = cart;
            }
            return RedirectToAction("Index");
        }

        public IActionResult ClearCartSession()
        {
            ClearCart();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var customerId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID)?.Value;
                var khachhang = model.GiongKhachHang ? _db.Customers.SingleOrDefault(kh => kh.CustomerId == customerId) : null;
                var totalAmount = Cart.Sum(item => item.SoLuong * item.Dongia);
                var hoadon = new Order
                {
                    CustomerId = customerId,
                    FullName = model.FullName ?? khachhang?.FullName,
                    Address = model.Address ?? khachhang?.Address,
                    Phone = model.Phone ?? khachhang?.Phone,
                    PaymentDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    CachThanhToan = "COD",
                    TongTien = totalAmount
                };

                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.Add(hoadon);
                        _db.SaveChanges(); 

                        var orderId = hoadon.OrderID;

                        var cthds = Cart.Select(item => new OrderDetail
                        {
                            OrderID = orderId,
                            Quantity = item.SoLuong,
                            Total = item.Dongia,
                            ProductId = item.MaHh,
                    
                        }).ToList();

                        _db.AddRange(cthds);
                        _db.SaveChanges(); 

                        transaction.Commit(); 
                        ClearCart();
                        return View("Success");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); 
                        TempData["Error"] = "Có lỗi xảy ra khi thanh toán đơn hàng. Vui lòng thử lại sau.";
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return View(Cart);
        }

    }
}