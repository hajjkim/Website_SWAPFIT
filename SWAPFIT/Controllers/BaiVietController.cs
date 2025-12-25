using SWAPFIT.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SWAPFIT.Controllers
{
    public class BaiVietController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BaiVietController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Create()
        {
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");

            var baiViet = new BaiViet
            {
                AnhBaiViets = new List<AnhBaiViet>()
            };

            return View(baiViet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BaiViet baiViet, List<IFormFile> AnhSanPham)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                TempData["Error"] = "Vui lòng đăng nhập để đăng bài!";
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                baiViet.MaNguoiDung = maNguoiDung.Value;
                baiViet.NgayTao = DateTime.Now;
                baiViet.TrangThai = "Chờ duyệt";
                if (baiViet.LoaiBaiDang == "Tặng")
                    baiViet.GiaSanPham = 0;
                _context.BaiViets.Add(baiViet);
                await _context.SaveChangesAsync();
                if (AnhSanPham != null && AnhSanPham.Count > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "AnhBaiViet");
                    Directory.CreateDirectory(uploadFolder); 
                    foreach (var file in AnhSanPham)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var anh = new AnhBaiViet
                            {
                                MaBaiViet = baiViet.MaBaiViet,
                                DuongDan = "/Upload/AnhBaiViet/" + fileName
                            };
                            _context.AnhBaiViets.Add(anh);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Đăng bài thành công! Đang chờ duyệt...";
                return RedirectToAction("ThongBaoChoDuyet");
            }
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
            return View(baiViet);
        }
        public IActionResult ThongBaoChoDuyet()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetSizesByCategory(int categoryId)
        {
            var category = _context.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == categoryId);

            if (category == null)
                return Json(new List<string>());

            string name = category.TenDanhMuc.ToLower();
            List<string> sizes = new List<string>();

            if (name.Contains("áo") || name.Contains("quần") || name.Contains("váy"))
            {
                sizes = new List<string> { "S", "M", "L", "XL", "XXL" };
            }
            else if (name.Contains("giày") || name.Contains("dép"))
            {
                sizes = Enumerable.Range(35, 10).Select(i => i.ToString()).ToList();
            }

            return Json(sizes);
        }
        private void LoadDropdownData()
        {
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
        }
    }
}
