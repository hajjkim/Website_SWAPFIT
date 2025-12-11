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

        // 🟢 Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
            {
                //TempData["Error"] = "Vui lòng đăng nhập để xem giỏ hàng!";
                return RedirectToAction("Login", "Account");
            }

<<<<<<< HEAD
            // Clear temporary cart (GioHangTam) from session when accessing the main cart page
            HttpContext.Session.Remove("GioHangTam");

=======
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(c => c.BaiViet)
                .ThenInclude(bv => bv.AnhBaiViets) // 🟢 thêm dòng này
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

        // 🟢 Thêm sản phẩm vào giỏ hàng
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
                _context.SaveChanges(); // ⚡ Bắt buộc có dòng này để tạo MaGioHang thật
            }

            // ✅ Gọi lại ID đã có thật trong DB
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


        // 🟢 Xóa sản phẩm khỏi giỏ hàng
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

            // ⚡ Kiểm tra số lượng tối đa dựa trên BaiViet.SoLuong
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

            // Tạo model tạm để hiển thị
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

        // ------------------------------------------------------------------------------------
        // 🟢 3) XỬ LÝ ĐẶT HÀNG (MUA NGAY)
        // ------------------------------------------------------------------------------------
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

<<<<<<< HEAD
            Console.WriteLine($"Đặt hàng: MaNguoiBan={model.MaNguoiBan}, TongTien={model.TongTien}");

=======
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            _context.DonHangs.Add(model);
            await _context.SaveChangesAsync();

            // Lưu chi tiết
            foreach (var ct in model.ChiTietDonHangs)
            {
                ct.MaDonHang = model.MaDonHang;
                _context.ChiTietDonHangs.Add(ct);

                // Trừ kho sản phẩm
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
<<<<<<< HEAD
=======

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
    }

}

