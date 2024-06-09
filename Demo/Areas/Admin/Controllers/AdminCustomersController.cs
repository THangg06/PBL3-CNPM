﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Data;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.Identity.Client;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCustomersController : Controller
    {
        private readonly Web01Context _context;
        public INotyfService _notyfService { get; }
        public AdminCustomersController(Web01Context context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCustomers
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 100000;
            var IsCustomers = _context.Customers.AsNoTracking().
                OrderByDescending(x => x.CreateDate);
            PagedList<Customer> models = new PagedList<Customer>(IsCustomers, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [HttpPost]
        public JsonResult UpdateTT(string id, bool trangthai)
        {
            try
            {
                var item = _context.Customers.Find(id);
                if (item == null)
                {
                    return Json(new { message = "Customer not found", Success = false });
                }

                item.Active = trangthai;
                _context.Entry(item).Property(x => x.Active).IsModified = true; // Đánh dấu thuộc tính Active là đã sửa đổi
                _context.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: log to a file, database, or logging service
                System.Diagnostics.Debug.WriteLine("Error updating customer: " + ex.Message);
                return Json(new { message = "Error: " + ex.Message, Success = false });
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/AdminCustomers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FullName,Birthday,Avatar,Address,Email,Phone,CreateDate,Password,Salt,LastLogin,Active")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Admin/AdminCustomers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Description", customer.RoleId);

            return View(customer);
        }// POST: Admin/AdminCustomers/Edit/5
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CustomerId,FullName,Address,Birthday,Avatar,RoleId,Email,Phone,CreateDate,Password,Salt,LastLogin,Active")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if RoleId is 1
                    if (customer.RoleId == 1)
                    {
                        // Create a new account for the customer
                        var newAccount = new Account
                        {
                            AccountId = customer.CustomerId,
                            FullName = customer.FullName,
                            Phone = customer.Phone,
                            Email = customer.Email,
                            Password = customer.Password,
                            Salt = customer.Salt,
                            Avatar = customer.Avatar,
                            Active = customer.Active,
                            RoleId = customer.RoleId,
                            CreateDate = customer.CreateDate,
                        };

                        _context.Accounts.Add(newAccount);

                        var customerOrders = _context.Orders.Where(o => o.CustomerId == customer.CustomerId);

                        foreach (var order in customerOrders.ToList())
                        {
                            var orderDetails = _context.OrderDetails.Where(od => od.OrderID == order.OrderID).ToList();
                            _context.OrderDetails.RemoveRange(orderDetails);
                        }

                        _context.Orders.RemoveRange(customerOrders);

                        await _context.SaveChangesAsync();

                        var existingCustomer = await _context.Customers.FindAsync(customer.CustomerId);
                        if (existingCustomer != null)
                        {
                            _context.Customers.Remove(existingCustomer);
                            await _context.SaveChangesAsync();
                        }

                        _notyfService.Success("Chỉnh sửa thành công");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    _notyfService.Success("Chỉnh sửa thất bại");
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Description", customer.RoleId);

            return View(customer);
        }




        // GET: Admin/AdminCustomers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/AdminCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var customer = await _context.Customers.FindAsync(id);
           

            if (customer != null)
            {
                var customerOrders = _context.Orders.Where(o => o.CustomerId == customer.CustomerId);

                foreach (var order in customerOrders.ToList())
                {
                    var orderDetails = _context.OrderDetails.Where(od => od.OrderID == order.OrderID).ToList();
                    _context.OrderDetails.RemoveRange(orderDetails);
                }

                _context.Orders.RemoveRange(customerOrders);

                await _context.SaveChangesAsync();

                _context.Customers.Remove(customer);
                //customer.Active = false;
                //customer.LastLogin = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("Tài khoản khách hàng khóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}