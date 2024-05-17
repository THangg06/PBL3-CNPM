using Demo.Data;
using Demo.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Demo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers
{
    public class CartController : Controller
    {
        private readonly Web01Context db;

        public CartController(Web01Context context)
        {
            db = context;
        }
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        private List<CartItem> GetCartFromSession()
        {
            return HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            HttpContext.Session.Set(MySetting.CART_KEY, cart);
        }

        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        public IActionResult AddToCart(string id, int quantity = 1)
        {
            var cart = GetCartFromSession();
            var item = cart.SingleOrDefault(p => p.MaHh.ToString() == id);

            if (item == null)
            {
                var product = db.Products.SingleOrDefault(p => p.ProductId == id);

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

            SaveCartToSession(cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(string id)
        {
            var cart = GetCartFromSession();
            var item = cart.SingleOrDefault(p => p.MaHh.ToString() == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(MySetting.CART_KEY);
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
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID)?.Value;
                var khachhang = new Customer();
                if (model.GiongKhachHang)
                {
                    khachhang = db.Customers.SingleOrDefault(kh => kh.CustomerId == customerId);
                }

                var hoadon = new Order
                {
                    CustomerId = customerId,
                    FullName = model.FullName ?? khachhang.FullName,
                    Address = model.Address ?? khachhang.Address,
                    Phone = model.Phone ?? khachhang.Phone,
                    PaymentDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    CachThanhToan = "COD",
                };

                db.Database.BeginTransaction();
                try
                {
                    db.Add(hoadon);
                    db.SaveChanges(); // Save the order to get a generated OrderId

                    var cthds = new List<OrderDetail>();
                    foreach (var item in Cart)
                    {
                        cthds.Add(new OrderDetail
                        {
                            OrderID = hoadon.OrderID, // Set OrderId after saving order
                            Quantity = item.SoLuong,
                            Total = item.Dongia,
                            ProductId = item.MaHh,
                        });
                    }
                    db.AddRange(cthds);
                    db.SaveChanges();

                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    return View("Success");
                }
                catch
                {
                    db.Database.RollbackTransaction();
                    // Handle error (optional)
                }
            }

            return View(Cart);
        }
    }
}
