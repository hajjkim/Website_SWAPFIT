using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Data;
using SWAPFIT.Model; // Correct namespace for UserVoucher
using SWAPFIT.Models; // Assuming the models like UuDai and NguoiDung are in this namespace

public class UuDaiController : Controller
{
    private readonly ApplicationDbContext _context;

    public UuDaiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Thêm ưu đãi mới
    [HttpPost]
    public IActionResult CreateUuDai(string tenUuDai, string moTa, string loaiUuDai, decimal giaTri, DateTime ngayBatDau, DateTime ngayKetThuc)
    {
        var uuDai = new UuDai
        {
            TenUuDai = tenUuDai,
            MoTa = moTa,
            LoaiUuDai = loaiUuDai,
            GiaTri = giaTri,
            NgayBatDau = ngayBatDau,
            NgayKetThuc = ngayKetThuc,
            TrangThai = "HoatDong" // Trạng thái mặc định là hoạt động
        };

        _context.UuDais.Add(uuDai);
        _context.SaveChanges();

        TempData["Success"] = "Ưu đãi đã được tạo thành công!";
        return RedirectToAction("ManageUuDai");
    }

    // Lấy danh sách tất cả các ưu đãi
    public IActionResult ManageUuDai()
    {
        var uuDais = _context.UuDais.ToList();
        return View(uuDais);
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
            DateClaimed = DateTime.Now
        };

        _context.UserVouchers.Add(userVoucher);
        _context.SaveChanges();

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
}
