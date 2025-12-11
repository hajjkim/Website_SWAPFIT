using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Models;
using SWAPFIT.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

<<<<<<< HEAD
=======
// ⭐ thêm namespace để dùng Claims + Cookie Authentication
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
namespace SWAPFIT.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
        // 🟢 GET: /Account/Login
        // 🟢 GET: /Account/Login
=======
        // ============================
        // 🟢 GET: LOGIN
        // ============================
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
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

<<<<<<< HEAD
        // 🟢 POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string tenDangNhap, string matKhau)
=======
        // ============================
        // 🟢 POST: LOGIN
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string tenDangNhap, string matKhau)
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                ViewBag.Error = "⚠️ Vui lòng nhập đầy đủ thông tin đăng nhập.";
                return View();
            }

            var user = _context.NguoiDungs
                .FirstOrDefault(u =>
<<<<<<< HEAD
                    u.TenDangNhap.ToLower() == tenDangNhap.Trim().ToLower() &&
                    u.MatKhau == matKhau.Trim()
=======
                    u.TenDangNhap.ToLower() == tenDangNhap.Trim().ToLower()
                    && u.MatKhau == matKhau.Trim()
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
                );

            if (user == null)
            {
                ViewBag.Error = "❌ Sai tên đăng nhập hoặc mật khẩu!";
                return View();
            }

<<<<<<< HEAD
            // Cho phép admin đăng nhập dù trạng thái là gì
            if (user.VaiTro?.ToLower() != "admin")
            {
                if (user.TrangThai == null || user.TrangThai != "Hoạt động")
                {
                    ViewBag.Error = "🚫 Tài khoản của bạn đang bị khóa hoặc chưa được kích hoạt.";
=======
            // Chặn user bị khóa (trừ admin)
            if (user.VaiTro?.ToLower() != "admin")
            {
                if (user.TrangThai != "Hoạt động")
                {
                    ViewBag.Error = "🚫 Tài khoản của bạn đang bị khóa hoặc chưa kích hoạt.";
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
                    return View();
                }
            }

<<<<<<< HEAD
=======
            // ================================
            // ⭐ Thêm CLAIMS cho SignalR nhận diện USER
            // ================================
            var claims = new List<Claim>
            {
                new Claim("MaNguoiDung", user.MaNguoiDung.ToString()),
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro ?? "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(12)
                }
            );
            // ================================

            // 🟢 Lưu SESSION như cũ
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            HttpContext.Session.SetInt32("MaNguoiDung", user.MaNguoiDung);
            HttpContext.Session.SetString("TenDangNhap", user.TenDangNhap);
            HttpContext.Session.SetString("Role", user.VaiTro.ToLower());

            if (user.VaiTro.ToLower() == "admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Home");
        }

<<<<<<< HEAD

        // 🟢 GET: /Account/Register
=======
        // ============================
        // 🟢 GET: REGISTER
        // ============================
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

<<<<<<< HEAD
        // 🟢 POST: /Account/Register
=======
        // ============================
        // 🟢 POST: REGISTER
        // ============================
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(NguoiDung model, IFormFile? anhDaiDien)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin hợp lệ!";
                return View(model);
            }

<<<<<<< HEAD
            // Kiểm tra trùng tên đăng nhập hoặc email
=======
            // Kiểm tra trùng tên đăng nhập
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            if (_context.NguoiDungs.Any(u => u.TenDangNhap == model.TenDangNhap))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View(model);
            }
<<<<<<< HEAD
            if (!string.IsNullOrEmpty(model.Email) && _context.NguoiDungs.Any(u => u.Email == model.Email))
=======

            // Kiểm tra trùng email
            if (!string.IsNullOrEmpty(model.Email) &&
                _context.NguoiDungs.Any(u => u.Email == model.Email))
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            {
                ViewBag.Error = "Email đã được sử dụng!";
                return View(model);
            }

<<<<<<< HEAD
            // Xử lý upload ảnh đại diện
            if (anhDaiDien != null && anhDaiDien.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
=======
            // Upload avatar
            if (anhDaiDien != null && anhDaiDien.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "uploads", "avatars");

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
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

<<<<<<< HEAD
            // Gán thông tin mặc định
=======
            // Giá trị mặc định
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            model.NgayTao = DateTime.Now;
            model.VaiTro = "User";
            model.TrangThai = "Hoạt động";

            _context.NguoiDungs.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

<<<<<<< HEAD
        // 🟢 GET: /Account/Logout
        public IActionResult Logout()
        {
=======
        // ============================
        // 🟢 LOGOUT
        // ============================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
