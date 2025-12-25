using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Models;
using SWAPFIT.Data;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SWAPFIT.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
            {
                return RedirectToAction("Login", "Account");
            }

            HttpContext.Session.Remove("GioHangTam");

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(c => c.BaiViet)
                .ThenInclude(bv => bv.AnhBaiViets) 
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    MaNguoiDung = maNguoiDung.Value,
                    NgayCapNhat = DateTime.Now,
                    ChiTietGioHangs = new List<ChiTietGioHang>()
                };
                _context.GioHangs.Add(gioHang);
                _context.SaveChanges();
            }

            return View(gioHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemVaoGio(int maBaiViet)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ!";
                return RedirectToAction("Login", "Account");
            }

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    MaNguoiDung = maNguoiDung.Value,
                    NgayCapNhat = DateTime.Now,
                    ChiTietGioHangs = new List<ChiTietGioHang>()
                };

                _context.GioHangs.Add(gioHang);
                _context.SaveChanges(); 
            }

            var chiTiet = _context.ChiTietGioHangs
                .FirstOrDefault(c => c.MaGioHang == gioHang.MaGioHang && c.MaBaiViet == maBaiViet);

            if (chiTiet == null)
            {
                chiTiet = new ChiTietGioHang
                {
                    MaGioHang = gioHang.MaGioHang,
                    MaBaiViet = maBaiViet,
                    SoLuong = 1
                };
                _context.ChiTietGioHangs.Add(chiTiet);
            }
            else
            {
                chiTiet.SoLuong++;
                _context.ChiTietGioHangs.Update(chiTiet);
            }

            gioHang.NgayCapNhat = DateTime.Now;
            _context.SaveChanges();

            TempData["Success"] = "🛒 Sản phẩm đã được thêm vào giỏ hàng!";
            Console.WriteLine($"Gio hang: {gioHang.MaGioHang}, ChiTiet: {gioHang.ChiTietGioHangs.Count}");

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult XoaSanPham(int maChiTiet)
        {
            var chiTiet = _context.ChiTietGioHangs.Find(maChiTiet);
            if (chiTiet != null)
            {
                _context.ChiTietGioHangs.Remove(chiTiet);
                _context.SaveChanges();
            }

            TempData["Success"] = "🗑️ Đã xóa sản phẩm khỏi giỏ hàng!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CapNhatSoLuong(int maChiTiet, int soLuongMoi)
        {
            var chiTiet = _context.ChiTietGioHangs
                .Include(c => c.BaiViet)
                .FirstOrDefault(c => c.MaChiTiet == maChiTiet);

            if (chiTiet == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
                return RedirectToAction("Index");
            }

            if (soLuongMoi < 1)
            {
                TempData["Error"] = "Số lượng phải lớn hơn 0.";
                return RedirectToAction("Index");
            }

            if (soLuongMoi > chiTiet.BaiViet.SoLuong)
            {
                TempData["Error"] = $"Chỉ còn {chiTiet.BaiViet.SoLuong} sản phẩm trong kho.";
                return RedirectToAction("Index");
            }

            chiTiet.SoLuong = soLuongMoi;
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật số lượng thành công!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ThanhToan(int id)
        {
            var baiViet = await _context.BaiViets
                .Include(x => x.AnhBaiViets)
                .FirstOrDefaultAsync(x => x.MaBaiViet == id);

            if (baiViet == null) return NotFound();

            var model = new DonHang
            {
                MaNguoiBan = baiViet.MaNguoiDung,
                DiaChiGiaoHang = "",
                PhuongThucThanhToan = "COD",
                PhuongThucGiaoHang = "Giao tận nơi",
                TongTien = baiViet.GiaSanPham ?? 0,
                ChiTietDonHangs = new List<ChiTietDonHang>
                {
                    new ChiTietDonHang
                    {
                        MaBaiViet = baiViet.MaBaiViet,
                        Gia = baiViet.GiaSanPham ?? 0,
                        SoLuong = 1,
                        BaiViet = baiViet
                    }
                }
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatHang(DonHang model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin!";
                return View("ThanhToan", model);
            }

            model.NgayDat = DateTime.Now;
            model.TrangThai = "Đang xử lý";

            Console.WriteLine($"Đặt hàng: MaNguoiBan={model.MaNguoiBan}, TongTien={model.TongTien}");

            _context.DonHangs.Add(model);
            await _context.SaveChangesAsync();

            foreach (var ct in model.ChiTietDonHangs)
            {
                ct.MaDonHang = model.MaDonHang;
                _context.ChiTietDonHangs.Add(ct);

                var sp = await _context.BaiViets.FindAsync(ct.MaBaiViet);
                if (sp != null)
                {
                    sp.SoLuong -= ct.SoLuong;
                }
            }

            await _context.SaveChangesAsync();

            TempData["ThongBaoDonHang"] = "Đơn hàng của bạn đã được đặt và đang chờ xử lý!";
            return RedirectToAction("HoanTat");

        }

        public IActionResult HoanTat()
        {
            return View();
        }
    }

}

