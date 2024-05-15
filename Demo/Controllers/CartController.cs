
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
		private readonly Web01Context _db;

		public CartController(Web01Context context)
		{
			_db = context;
		}
		public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
		//const string MySetting.CART_KEY = "MYCART";

		// Sử dụng một phương thức riêng để truy cập session
		private List<CartItem> GetCartFromSession()
		{
			return HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
		}

		// Sử dụng một phương thức riêng để lưu giỏ hàng vào session
		private void SaveCartToSession(List<CartItem> cart)
		{
			HttpContext.Session.Set(MySetting.CART_KEY, cart);
		}

		public IActionResult Index()
		{
			// Lấy giỏ hàng từ session
			var cart = GetCartFromSession();
			return View(cart);
		}

		public IActionResult AddToCart(string id, int quantity = 1)
		{
			Console.WriteLine($"ID of the product: {id}");

			// Lấy giỏ hàng từ session
			var cart = GetCartFromSession();

			// Tìm kiếm sản phẩm trong giỏ hàng
			var item = cart.SingleOrDefault(p => p.MaHh.ToString() == id);

			if (item == null)
			{
				// Nếu sản phẩm không tồn tại trong giỏ hàng, thì tìm trong cơ sở dữ liệu
				var product = _db.Products.SingleOrDefault(p => p.ProductId == id);

				if (product == null)
				{
					TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
					return Redirect("/404");
				}

				// Tạo mới một CartItem và thêm vào giỏ hàng
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
				// Nếu sản phẩm đã tồn tại trong giỏ hàng, cập nhật số lượng
				item.SoLuong += quantity;
			}

			// Lưu giỏ hàng mới vào session
			SaveCartToSession(cart);

			// Chuyển hướng người dùng đến trang giỏ hàng
			return RedirectToAction("Index");
		}
		public IActionResult RemoveCart(string id)
		{
			var cart = GetCartFromSession();

			// Tìm kiếm sản phẩm trong giỏ hàng
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
			// Xóa toàn bộ các mục khỏi giỏ hàng trong session
			HttpContext.Session.Remove(MySetting.CART_KEY);

			// Hoặc nếu bạn muốn xóa giỏ hàng và tạo ra một giỏ hàng mới trống
			// HttpContext.Session.Set(MySetting.CART_KEY, new List<CartItem>());

			// Sau đó chuyển hướng người dùng đến trang chủ hoặc trang giỏ hàng
			return RedirectToAction("Index", "Home");
		}
		[Authorize]
		[HttpGet]
		public IActionResult Checkout()
		{
			//var giohang = Cart;
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
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
                var khachhang = new Customer();
                if (model.GiongKhachHang)
                {
                    khachhang = _db.Customers.SingleOrDefault(kh => kh.CustomerId == customerId);
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

                _db.Database.BeginTransaction();
                try
                {
                    _db.Database.CommitTransaction();
                    _db.Add(hoadon);
                    _db.SaveChanges();

                    var cthds = new List<OrderDetail>();
                    foreach (var item in Cart)
                    {
                        cthds.Add(new OrderDetail
                        {
                            OrderId = hoadon.OrderId,
                            Quantity = item.SoLuong,
                            Total = item.Dongia,
                            ProductId = item.MaHh,
                        });
                    }
                    _db.AddRange(cthds);
                    _db.SaveChanges();

                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    return View("Success");
                }
                catch
                {
                    _db.Database.RollbackTransaction();
                }
            }

            return View(Cart);
        }

        //    if (ModelState.IsValid)
        //    {
        //        var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID)?.Value;

        //        _db.Database.BeginTransaction();

        //        try
        //        {
        //            var hoadon = new Order
        //            {
        //                CustomerId = customerId,
        //                Address = model.Address,
        //                Phone = model.Phone,
        //                PaymentDate = DateTime.Now,
        //                CachThanhToan = "COD",
        //                // Các trường khác có thể thêm vào dựa trên model hoặc thông tin khác
        //            };

        //            _db.Add(hoadon);
        //            _db.SaveChanges();

        //            var cthds = new List<OrderDetail>();
        //            foreach (var item in Cart)
        //            {
        //                cthds.Add(new OrderDetail
        //                {
        //                    OrderId = hoadon.OrderId,
        //                    Quantity = item.SoLuong,
        //                    Total = item.Dongia,
        //                    ProductId = item.MaHh,
        //                    // Các trường khác có thể thêm vào dựa trên model hoặc thông tin khác
        //                });
        //            }

        //            _db.AddRange(cthds);
        //            _db.SaveChanges();

        //            HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

        //            _db.Database.CommitTransaction();

        //            return View("Success");
        //        }
        //        catch
        //        {
        //            _db.Database.RollbackTransaction();
        //            // Xử lý khi có lỗi xảy ra
        //        }
        //    }
        //    else
        //    {
        //        return View(model);
        //    }
        //   // ClearCart();

        //    // Chuyển hướng về trang chủ
        //  //  return RedirectToAction("Index", "Home", new { success = true });
        //     return View("Success");
        //    return View(Cart);
        //}


    }
}


