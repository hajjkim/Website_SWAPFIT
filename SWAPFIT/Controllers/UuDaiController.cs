<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Data;
using SWAPFIT.Model;   // UserVoucher
using SWAPFIT.Models; // UuDai, DonHang, BaiViet,...
=======
﻿using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Data;
using SWAPFIT.Model; // Correct namespace for UserVoucher
using SWAPFIT.Models; // Assuming the models like UuDai and NguoiDung are in this namespace
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff

public class UuDaiController : Controller
{
    private readonly ApplicationDbContext _context;

    public UuDaiController(ApplicationDbContext context)
    {
        _context = context;
    }

<<<<<<< HEAD
    // =========================================================
    // 🔹 1. USER – TRANG DANH SÁCH VOUCHER CÓ THỂ NHẬN
    // =========================================================
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

        return View(uuDaiDangHoatDong); // Views/UuDai/Index.cshtml
    }

    // =========================================================
    // 🔹 2. ADMIN – TẠO ƯU ĐÃI MỚI
    // =========================================================
    [HttpPost]
    public IActionResult CreateUuDai(
        string tenUuDai,
        string moTa,
        string loaiUuDai,
        decimal giaTri,
        DateTime ngayBatDau,
        DateTime ngayKetThuc)
=======
    // Thêm ưu đãi mới
    [HttpPost]
    public IActionResult CreateUuDai(string tenUuDai, string moTa, string loaiUuDai, decimal giaTri, DateTime ngayBatDau, DateTime ngayKetThuc)
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
    {
        var uuDai = new UuDai
        {
            TenUuDai = tenUuDai,
            MoTa = moTa,
            LoaiUuDai = loaiUuDai,
            GiaTri = giaTri,
            NgayBatDau = ngayBatDau,
            NgayKetThuc = ngayKetThuc,
<<<<<<< HEAD
            TrangThai = "HoatDong" // mặc định
=======
            TrangThai = "HoatDong" // Trạng thái mặc định là hoạt động
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        };

        _context.UuDais.Add(uuDai);
        _context.SaveChanges();

        TempData["Success"] = "Ưu đãi đã được tạo thành công!";
<<<<<<< HEAD
        return RedirectToAction(nameof(ManageUuDai));
    }

    // Danh sách tất cả ưu đãi (admin)
    public IActionResult ManageUuDai()
    {
        var uuDais = _context.UuDais
            .OrderByDescending(v => v.NgayBatDau)
            .ToList();
        return View(uuDais);
    }

    // =========================================================
    // 🔹 3. ADMIN – CHỈNH SỬA / BẬT TẮT ƯU ĐÃI
    // =========================================================

    // GET: form sửa
    [HttpGet]
    public IActionResult EditUuDai(int id)
    {
        var uuDai = _context.UuDais.Find(id);
        if (uuDai == null) return NotFound();

        return View(uuDai); // Views/UuDai/EditUuDai.cshtml
    }

    // POST: lưu sửa
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

    // Bật / tắt nhanh
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

=======
        return RedirectToAction("ManageUuDai");
    }

    // Lấy danh sách tất cả các ưu đãi
    public IActionResult ManageUuDai()
    {
        var uuDais = _context.UuDais.ToList();
        return View(uuDais);
    }

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
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

<<<<<<< HEAD
        return RedirectToAction(nameof(ManageUuDai));
    }

    // =========================================================
    // 🔹 4. USER – LƯU VOUCHER (cách dùng Ajax)
    [HttpPost]
    [IgnoreAntiforgeryToken] // vì ta không gửi token
    public IActionResult SaveVoucher(int voucherId)
    {
        var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        if (!userId.HasValue)
            return Json(new { success = false, message = "Bạn cần đăng nhập để lưu voucher!" });

        var now = DateTime.Now;
        var voucher = _context.UuDais.FirstOrDefault(v =>
    v.MaUuDai == voucherId &&
    v.TrangThai.Trim() == "HoatDong" &&
    v.NgayBatDau <= DateTime.Now.AddMonths(12) &&  // cho phép voucher tương lai
    v.NgayKetThuc >= DateTime.Now.AddMonths(-12)); // chấp nhận quá khứ

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

    // =========================================================
    // 🔹 5. USER – LƯU VOUCHER (click nút, redirect lại trang)
    // =========================================================
    // ⭐ Action LƯU VOUCHER VÀO VÍ NGƯỜI DÙNG
    // ⭐ Action LƯU VOUCHER VÀO VÍ NGƯỜI DÙNG
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClaimVoucher(int id)
    {
        var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        if (maNguoiDung == null)
            return RedirectToAction("Login", "Account");

        // 1. Lấy voucher đang hoạt động + còn trong thời gian
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

        // 2. Check GIỚI HẠN SỐ LƯỢNG (tổng số user có thể nhận)
        if (voucher.GioiHanSoLuong.HasValue)
        {
            var soDaNhan = _context.UserVouchers.Count(uv => uv.VoucherId == id);
            if (soDaNhan >= voucher.GioiHanSoLuong.Value)
            {
                TempData["Error"] = "Voucher này đã hết số lượng.";
                return RedirectToAction("Index");
            }
        }

        // 3. Check 1 USER chỉ được lưu voucher này 1 lần
        bool daNhan = _context.UserVouchers
            .Any(uv => uv.UserId == maNguoiDung.Value && uv.VoucherId == id);

        if (daNhan)
        {
            TempData["Error"] = "Bạn đã lưu voucher này rồi.";
            return RedirectToAction("Index");
        }

        // 4. Lưu vào bảng UserVouchers
        var userVoucher = new UserVoucher
        {
            UserId = maNguoiDung.Value,
            VoucherId = id,
=======
        return RedirectToAction("ManageUuDai");
    }

    // Lưu voucher
    [HttpPost]
    public IActionResult SaveVoucher(int voucherId)
    {
        var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        if (userId == null)
        {
            TempData["Error"] = "Bạn cần đăng nhập để lưu voucher.";
            return Json(new { success = false, message = "Bạn cần đăng nhập để lưu voucher." });
        }

        // Kiểm tra nếu voucher tồn tại
        var voucher = _context.UuDais.FirstOrDefault(u => u.MaUuDai == voucherId);
        if (voucher == null)
        {
            TempData["Error"] = "Voucher không hợp lệ.";
            return Json(new { success = false, message = "Voucher không hợp lệ." });
        }

        // Kiểm tra nếu người dùng đã nhận voucher này
        var existingClaim = _context.UserVouchers
            .FirstOrDefault(uv => uv.UserId == userId && uv.VoucherId == voucherId);
        if (existingClaim != null)
        {
            return Json(new { success = false, message = "Voucher đã được lưu trước đó." });
        }

        // Lưu voucher vào bảng UserVoucher
        var userVoucher = new UserVoucher
        {
            UserId = (int)userId,
            VoucherId = voucherId,
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            DateClaimed = DateTime.Now
        };

        _context.UserVouchers.Add(userVoucher);
        _context.SaveChanges();

<<<<<<< HEAD
        TempData["Success"] = "Đã lưu voucher vào ví của bạn!";
        return RedirectToAction("Index");
    }


    // =========================================================
    // 🔹 6. USER – XEM “VÍ VOUCHER” CỦA MÌNH
    // =========================================================
    public IActionResult MyVouchers()
    {
        var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        if (maNguoiDung == null)
            return RedirectToAction("Login", "Account");

        var now = DateTime.Now;

        var myVouchers = _context.UserVouchers
            .Where(uv => uv.UserId == maNguoiDung.Value)
            .Include(uv => uv.Voucher) // navigation trong UserVoucher
            .OrderByDescending(uv => uv.DateClaimed)
            .ToList();

        // View: Views/UuDai/MyVouchers.cshtml
        // bạn có thể hiển thị: Tên, Mô tả, Hết hạn chưa, v.v.
        return View(myVouchers);
    }

    // User xóa 1 voucher khỏi ví (tuỳ chọn)
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

    // =========================================================
    // 🔹 7. ÁP DỤNG VOUCHER VÀO ĐƠN HÀNG (ví dụ)
    // =========================================================
    // action này bạn có thể gọi bằng Ajax ở trang Checkout
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
    

=======
        return Json(new { success = true, message = "Voucher đã được lưu vào kho của bạn!" });
    }



    // Apply voucher to order
    public IActionResult ApplyUuDai(int productId, string discountCode)
    {
        var uuDai = _context.UuDais.FirstOrDefault(u => u.TenUuDai == discountCode && u.TrangThai == "HoatDong" && u.NgayBatDau <= DateTime.Now && u.NgayKetThuc >= DateTime.Now);

        if (uuDai == null)
        {
            TempData["Error"] = "Mã giảm giá không hợp lệ hoặc đã hết hạn.";
            return RedirectToAction("Checkout");
        }

        var order = _context.DonHangs.FirstOrDefault(o => o.MaDonHang == productId);
        if (order == null)
        {
            TempData["Error"] = "Đơn hàng không hợp lệ.";
            return RedirectToAction("Checkout");
        }

        decimal totalOrderAmount = order.TongTien;

        var baiViet = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == productId);
        if (baiViet == null || baiViet.LoaiBaiDang != "Bán")
        {
            TempData["Error"] = "Mã giảm giá này chỉ áp dụng cho sản phẩm thuộc danh mục Thanh Lý.";
            return RedirectToAction("Checkout");
        }

        if (uuDai.LoaiUuDai == "PhanTram")
        {
            decimal discountAmount = (totalOrderAmount * uuDai.GiaTri / 100);
            decimal finalPrice = totalOrderAmount - discountAmount;
            TempData["Success"] = $"Áp dụng mã giảm giá thành công! Bạn đã tiết kiệm được {discountAmount} VND. Tổng giá trị đơn hàng cuối cùng: {finalPrice} VND.";
        }
        else
        {
            TempData["Error"] = "Voucher không hợp lệ cho loại ưu đãi này.";
            return RedirectToAction("Checkout");
        }

        return RedirectToAction("Checkout");
    }
    // ⭐ Action LƯU VOUCHER VÀO VÍ NGƯỜI DÙNG
    //[HttpPost]
    //public IActionResult ClaimVoucher(int id)
    //{
    //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
    //    if (maNguoiDung == null)
    //        return RedirectToAction("Login", "Account");

    //    var voucher = _context.UuDais
    //        .FirstOrDefault(v => v.MaUuDai == id && v.TrangThai == "HoatDong");

    //    if (voucher == null)
    //    {
    //        TempData["Error"] = "Voucher không tồn tại.";
    //        return RedirectToAction("Index");
    //    }

    //    // Nếu có giới hạn thì đếm số người đã nhận
    //    if (voucher.GioiHanSoLuong.HasValue)
    //    {
    //        var soDaNhan = _context.UserVouchers.Count(uv => uv.VoucherId == id);
    //        if (soDaNhan >= voucher.GioiHanSoLuong.Value)
    //        {
    //            TempData["Error"] = "Voucher này đã hết số lượng.";
    //            return RedirectToAction("Index");
    //        }
    //    }

    //    // Kiểm tra user đã có voucher này chưa
    //    bool daNhan = _context.UserVouchers
    //        .Any(uv => uv.UserId == maNguoiDung.Value && uv.VoucherId == id);

    //    if (daNhan)
    //    {
    //        TempData["Error"] = "Bạn đã lưu voucher này rồi.";
    //        return RedirectToAction("Index");
    //    }

    //    // Lưu vào bảng UserVouchers
    //    var userVoucher = new UserVoucher
    //    {
    //        UserId = maNguoiDung.Value,
    //        VoucherId = id,
    //        DateClaimed = DateTime.Now
    //    };
    //    _context.UserVouchers.Add(userVoucher);
    //    _context.SaveChanges();

    //    TempData["Success"] = "Đã lưu voucher vào ví của bạn!";
    //    return RedirectToAction("Index");
    //}
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
}
