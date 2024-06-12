using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Data;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Demo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly Web01Context _context;
        public INotyfService _notyfService { get; }
        public AdminAccountsController(Web01Context context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Index(bool? active)
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "Description");

            List<SelectListItem> IsTrangThai = new List<SelectListItem>();
            IsTrangThai.Add(new SelectListItem() { Text = "Active", Value = "true" });
            IsTrangThai.Add(new SelectListItem() { Text = "Block", Value = "false" });
            ViewData["IsTrangThai"] = IsTrangThai;

            var accountsQuery = _context.Accounts.AsNoTracking();

            if (active.HasValue)
            {
                accountsQuery = _context.Accounts.Where(a => a.Active == active.Value).Include(a => a.Role);
            }
            else
            {
                accountsQuery = accountsQuery.Include(a => a.Role);
            }

            var web01Context = await accountsQuery.ToListAsync();
            return View(web01Context);
        }



        public IActionResult Filtter(bool? active)
        {
            var url = active.HasValue ? $"/Admin/AdminAccounts?active={active}" : "/Admin/AdminAccounts";

            return Json(new { status = "success", redirectUrl = url });
        }


        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,FullName,Phone,Email,Password,Salt,Active,Avatar,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            var trangThaiList = new List<SelectListItem>
    {
        new SelectListItem { Text = "Active", Value = "true" },
        new SelectListItem { Text = "Inactive", Value = "false" }
    };

            ViewData["TrangThai"] = new SelectList(trangThaiList, "Value", "Text", account.Active);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AccountId,FullName,Phone,Email,Password,Salt,Avatar,Active,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        _notyfService.Success("Có lỗi xảy ra");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

      
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                account.Active = false;
                //_context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("Khóa tài khoản thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}