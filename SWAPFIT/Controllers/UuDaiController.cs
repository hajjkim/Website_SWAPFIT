using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Data;
using SWAPFIT.Model;   // UserVoucher
using SWAPFIT.Models; // UuDai, DonHang, BaiViet,...

public class UuDaiController : Controller
{
    private readonly ApplicationDbContext _context;

    public UuDaiController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var now = DateTime.Now;

        var uuDaiDangHoatDong = _context.UuDais
            .Where(v =>
                v.TrangThai == "HoatDong" &&
                v.NgayBatDau <= now &&
                v.NgayKetThuc >= now)
            .OrderByDescending(v => v.NgayBatDau)
            .ToList();

        return View(uuDaiDangHoatDong); 
    }
    [HttpPost]
    public IActionResult CreateUuDai(
        string tenUuDai,
        string moTa,
        string loaiUuDai,
        decimal giaTri,
        DateTime ngayBatDau,
        DateTime ngayKetThuc)
    {
        var uuDai = new UuDai
        {
            TenUuDai = tenUuDai,
            MoTa = moTa,
            LoaiUuDai = loaiUuDai,
            GiaTri = giaTri,
            NgayBatDau = ngayBatDau,
            NgayKetThuc = ngayKetThuc,
            TrangThai = "HoatDong" 
        };

        _context.UuDais.Add(uuDai);
        _context.SaveChanges();

        TempData["Success"] = "Ưu đãi đã được tạo thành công!";
        return RedirectToAction(nameof(ManageUuDai));
    }
    public IActionResult ManageUuDai()
    {
        var uuDais = _context.UuDais
            .OrderByDescending(v => v.NgayBatDau)
            .ToList();
        return View(uuDais);
    }
    [HttpGet]
    public IActionResult EditUuDai(int id)
    {
        var uuDai = _context.UuDais.Find(id);
        if (uuDai == null) return NotFound();

        return View(uuDai); 
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditUuDai(UuDai model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var uuDai = _context.UuDais.Find(model.MaUuDai);
        if (uuDai == null) return NotFound();

        uuDai.TenUuDai = model.TenUuDai;
        uuDai.MoTa = model.MoTa;
        uuDai.LoaiUuDai = model.LoaiUuDai;
        uuDai.GiaTri = model.GiaTri;
        uuDai.NgayBatDau = model.NgayBatDau;
        uuDai.NgayKetThuc = model.NgayKetThuc;
        uuDai.TrangThai = model.TrangThai;

        _context.SaveChanges();
        TempData["Success"] = "Cập nhật ưu đãi thành công!";
        return RedirectToAction(nameof(ManageUuDai));
    }
    [HttpPost]
    public IActionResult ToggleStatus(int id)
    {
        var uuDai = _context.UuDais.Find(id);
        if (uuDai == null) return NotFound();

        uuDai.TrangThai = uuDai.TrangThai == "HoatDong" ? "Ngung" : "HoatDong";
        _context.SaveChanges();

        TempData["Success"] = "Đã cập nhật trạng thái ưu đãi.";
        return RedirectToAction(nameof(ManageUuDai));
    }

    // Xóa ưu đãi
    [HttpPost]
    public IActionResult DeleteUuDai(int id)
    {
        var uuDai = _context.UuDais.Find(id);
        if (uuDai != null)
        {
            _context.UuDais.Remove(uuDai);
            _context.SaveChanges();
            TempData["Success"] = "Ưu đãi đã được xóa thành công!";
        }

        return RedirectToAction(nameof(ManageUuDai));
    }

    [HttpPost]
    [IgnoreAntiforgeryToken] 
    public IActionResult SaveVoucher(int voucherId)
    {
        var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        if (!userId.HasValue)
            return Json(new { success = false, message = "Bạn cần đăng nhập để lưu voucher!" });

        var now = DateTime.Now;
        var voucher = _context.UuDais.FirstOrDefault(v =>
    v.MaUuDai == voucherId &&
    v.TrangThai.Trim() == "HoatDong" &&
    v.NgayBatDau <= DateTime.Now.AddMonths(12) &&  
    v.NgayKetThuc >= DateTime.Now.AddMonths(-12)); 

        if (voucher == null)
            return Json(new { success = false, message = "Voucher không hợp lệ hoặc đã hết hạn!" });

        if (_context.UserVouchers.Any(x => x.UserId == userId.Value && x.VoucherId == voucherId))
            return Json(new { success = false, message = "Bạn đã lưu voucher này rồi!" });

        if (voucher.GioiHanSoLuong.HasValue)
        {
            var daNhan = _context.UserVouchers.Count(x => x.VoucherId == voucherId);
            if (daNhan >= voucher.GioiHanSoLuong.Value)
                return Json(new { success = false, message = "Voucher đã hết lượt nhận!" });
        }

        _context.UserVouchers.Add(new UserVoucher
        {
            UserId = userId.Value,
            VoucherId = voucherId,
            DateClaimed = DateTime.Now
        });
        _context.SaveChanges();

        return Json(new { success = true, message = "Đã lưu voucher thành công!" });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClaimVoucher(int id)
    {
        var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        if (maNguoiDung == null)
            return RedirectToAction("Login", "Account");
        var voucher = _context.UuDais
            .FirstOrDefault(v =>
                v.MaUuDai == id &&
                v.TrangThai == "HoatDong" &&
                v.NgayBatDau <= DateTime.Now &&
                v.NgayKetThuc >= DateTime.Now);

        if (voucher == null)
        {
            TempData["Error"] = "Voucher không tồn tại hoặc đã hết hạn.";
            return RedirectToAction("Index");
        }

        if (voucher.GioiHanSoLuong.HasValue)
        {
            var soDaNhan = _context.UserVouchers.Count(uv => uv.VoucherId == id);
            if (soDaNhan >= voucher.GioiHanSoLuong.Value)
            {
                TempData["Error"] = "Voucher này đã hết số lượng.";
                return RedirectToAction("Index");
            }
        }
        bool daNhan = _context.UserVouchers
            .Any(uv => uv.UserId == maNguoiDung.Value && uv.VoucherId == id);

        if (daNhan)
        {
            TempData["Error"] = "Bạn đã lưu voucher này rồi.";
            return RedirectToAction("Index");
        }
        var userVoucher = new UserVoucher
        {
            UserId = maNguoiDung.Value,
            VoucherId = id,
            DateClaimed = DateTime.Now
        };

        _context.UserVouchers.Add(userVoucher);
        _context.SaveChanges();

        TempData["Success"] = "Đã lưu voucher vào ví của bạn!";
        return RedirectToAction("Index");
    }
    public IActionResult MyVouchers()
    {
        var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        if (maNguoiDung == null)
            return RedirectToAction("Login", "Account");

        var now = DateTime.Now;

        var myVouchers = _context.UserVouchers
            .Where(uv => uv.UserId == maNguoiDung.Value)
            .Include(uv => uv.Voucher) 
            .OrderByDescending(uv => uv.DateClaimed)
            .ToList();
        return View(myVouchers);
    }
    [HttpPost]
    public IActionResult RemoveFromWallet(int id)
    {
        var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        if (maNguoiDung == null)
            return RedirectToAction("Login", "Account");

        var record = _context.UserVouchers
            .FirstOrDefault(uv => uv.VoucherId == id && uv.UserId == maNguoiDung.Value);

        if (record != null)
        {
            _context.UserVouchers.Remove(record);
            _context.SaveChanges();
            TempData["Success"] = "Đã xóa voucher khỏi ví.";
        }

        return RedirectToAction(nameof(MyVouchers));
    }
    [HttpPost]
    public IActionResult ApplyVoucherToTotal(int voucherId, decimal total)
    {
        var now = DateTime.Now;

        var voucher = _context.UuDais
            .FirstOrDefault(v => v.MaUuDai == voucherId &&
                                 v.TrangThai == "HoatDong" &&
                                 v.NgayBatDau <= now &&
                                 v.NgayKetThuc >= now);

        if (voucher == null)
        {
            return Json(new { success = false, message = "Voucher không hợp lệ hoặc đã hết hạn." });
        }

        decimal giam = 0;
        decimal thanhTien = total;

        if (voucher.LoaiUuDai == "PhanTram")
        {
            giam = total * voucher.GiaTri / 100m;
            thanhTien = total - giam;
        }
        else if (voucher.LoaiUuDai == "TienMat")
        {
            giam = voucher.GiaTri;
            thanhTien = Math.Max(0, total - giam);
        }

        return Json(new
        {
            success = true,
            discount = giam,
            finalTotal = thanhTien,
            message = $"Áp dụng voucher thành công! Giảm {giam:N0} đ"
        });

    }
    

}
