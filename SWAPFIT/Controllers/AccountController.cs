using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Models;
using SWAPFIT.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

namespace SWAPFIT.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🟢 GET: /Account/Login
        // 🟢 GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != null)
            {
                if (role.ToLower() == "admin")
                    return RedirectToAction("Dashboard", "Admin");

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // 🟢 POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                ViewBag.Error = "⚠️ Vui lòng nhập đầy đủ thông tin đăng nhập.";
                return View();
            }

            var user = _context.NguoiDungs
                .FirstOrDefault(u =>
                    u.TenDangNhap.ToLower() == tenDangNhap.Trim().ToLower() &&
                    u.MatKhau == matKhau.Trim()
                );

            if (user == null)
            {
                ViewBag.Error = "❌ Sai tên đăng nhập hoặc mật khẩu!";
                return View();
            }

            // Cho phép admin đăng nhập dù trạng thái là gì
            if (user.VaiTro?.ToLower() != "admin")
            {
                if (user.TrangThai == null || user.TrangThai != "Hoạt động")
                {
                    ViewBag.Error = "🚫 Tài khoản của bạn đang bị khóa hoặc chưa được kích hoạt.";
                    return View();
                }
            }

            HttpContext.Session.SetInt32("MaNguoiDung", user.MaNguoiDung);
            HttpContext.Session.SetString("TenDangNhap", user.TenDangNhap);
            HttpContext.Session.SetString("Role", user.VaiTro.ToLower());

            if (user.VaiTro.ToLower() == "admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Home");
        }


        // 🟢 GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 🟢 POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(NguoiDung model, IFormFile? anhDaiDien)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin hợp lệ!";
                return View(model);
            }

            // Kiểm tra trùng tên đăng nhập hoặc email
            if (_context.NguoiDungs.Any(u => u.TenDangNhap == model.TenDangNhap))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View(model);
            }
            if (!string.IsNullOrEmpty(model.Email) && _context.NguoiDungs.Any(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email đã được sử dụng!";
                return View(model);
            }

            // Xử lý upload ảnh đại diện
            if (anhDaiDien != null && anhDaiDien.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhDaiDien.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    anhDaiDien.CopyTo(stream);
                }

                model.AnhDaiDien = "/uploads/avatars/" + fileName;
            }

            // Gán thông tin mặc định
            model.NgayTao = DateTime.Now;
            model.VaiTro = "User";
            model.TrangThai = "Hoạt động";

            _context.NguoiDungs.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // 🟢 GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
