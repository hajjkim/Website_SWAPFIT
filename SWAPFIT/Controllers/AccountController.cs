using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Models;
using SWAPFIT.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using SWAPFIT.Model;

using System.Drawing;
using System.Drawing.Imaging;

namespace SWAPFIT.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

       
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


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(
       NguoiDung model,
       IFormFile? anhDaiDien,
       string? tenNganHang,
       string? soTaiKhoan,
       string? chuTaiKhoan,
       List<IFormFile>? qrImages  
   )
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin hợp lệ!";
                return View(model);
            }

            
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

            
            if (anhDaiDien != null && anhDaiDien.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(anhDaiDien.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    anhDaiDien.CopyTo(stream);

                model.AnhDaiDien = "/uploads/avatars/" + fileName;
            }

            model.NgayTao = DateTime.Now;
            model.VaiTro = "User";
            model.TrangThai = "Hoạt động";

            _context.NguoiDungs.Add(model);
            _context.SaveChanges();

            
            bool hasBank =
                !string.IsNullOrWhiteSpace(tenNganHang) ||
                !string.IsNullOrWhiteSpace(soTaiKhoan) ||
                !string.IsNullOrWhiteSpace(chuTaiKhoan) ||
                (qrImages != null && qrImages.Any(f => f != null && f.Length > 0));

            if (hasBank)
            {
                
                if (string.IsNullOrWhiteSpace(tenNganHang) ||
                    string.IsNullOrWhiteSpace(soTaiKhoan) ||
                    string.IsNullOrWhiteSpace(chuTaiKhoan))
                {
                    ViewBag.Error = "Vui lòng nhập đầy đủ thông tin ngân hàng (Tên ngân hàng, Số tài khoản, Chủ tài khoản).";
                    return View(model);
                }

                var bank = new TaiKhoanNganHang
                {
                    MaNguoiDung = model.MaNguoiDung,
                    TenNganHang = tenNganHang?.Trim(),
                    SoTaiKhoan = soTaiKhoan?.Trim(),
                    ChuTaiKhoan = chuTaiKhoan?.Trim(),
                };

              
                var savedQrPaths = new List<string>();

                if (qrImages != null)
                {
                    var validFiles = qrImages.Where(f => f != null && f.Length > 0).ToList();

                    if (validFiles.Count > 8)
                    {
                        ViewBag.Error = "Chỉ được tải lên tối đa 8 ảnh QR.";
                        return View(model);
                    }

                    string qrFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "qrcode");
                    if (!Directory.Exists(qrFolder)) Directory.CreateDirectory(qrFolder);

                    foreach (var file in validFiles)
                    {
                        var ext = Path.GetExtension(file.FileName).ToLower();
                        var allowed = new[] { ".png", ".jpg", ".jpeg", ".webp" };
                        if (!allowed.Contains(ext))
                        {
                            ViewBag.Error = "Chỉ cho phép ảnh: PNG/JPG/JPEG/WEBP.";
                            return View(model);
                        }

                        string fileName = $"qr_{model.MaNguoiDung}_{Guid.NewGuid():N}{ext}";
                        string filePath = Path.Combine(qrFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                            file.CopyTo(stream);

                        savedQrPaths.Add("/uploads/qrcode/" + fileName);
                    }
                }

                if (savedQrPaths.Count > 0)
                    bank.QrCodeImage = string.Join(";", savedQrPaths);

                _context.TaiKhoanNganHang.Add(bank);
                _context.SaveChanges();
            }

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
