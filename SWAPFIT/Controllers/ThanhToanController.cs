using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SWAPFIT.Data;
using SWAPFIT.Model;
using SWAPFIT.Models;
using System.Diagnostics;
using System.Text;

namespace SWAPFIT.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThanhToanController> _logger;

        public ThanhToanController(ApplicationDbContext context, ILogger<ThanhToanController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===============================
        // Trang xác nhận thanh toán (giỏ hàng bình thường)
        // ===============================
        //public IActionResult Index()
        //{
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (!maNguoiDung.HasValue)
        //        return RedirectToAction("Login", "Account");

        //    var gioHang = _context.GioHangs
        //        .Include(g => g.ChiTietGioHangs)
        //            .ThenInclude(ct => ct.BaiViet)
        //                .ThenInclude(b => b.AnhBaiViets)
        //        .Include(g => g.ChiTietGioHangs)
        //            .ThenInclude(ct => ct.BaiViet)
        //                .ThenInclude(b => b.NguoiDung)
        //        .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung.Value);

        //    var viewModel = new GioHangViewModel();

        //    if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
        //    {
        //        var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
        //        if (string.IsNullOrEmpty(gioHangTamJson))
        //        {
        //            TempData["Error"] = "Giỏ hàng của bạn đang trống!";
        //            return RedirectToAction("Index", "GioHang");
        //        }

        //        // Deserialize giỏ hàng tạm
        //        var gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(gioHangTamJson);
        //        viewModel.GioHangTam = gioHangTam;
        //    }
        //    else
        //    {
        //        viewModel.GioHang = gioHang;
        //    }

        //    return View(viewModel);
        //}
        //-----------------------------------------------------------------------------
        //public IActionResult Index()
        //{
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (!maNguoiDung.HasValue)
        //        return RedirectToAction("Login", "Account");

        //    int userId = maNguoiDung.Value;
        //    var gioHang = _context.GioHangs
        //        .Include(g => g.ChiTietGioHangs)
        //            .ThenInclude(ct => ct.BaiViet)
        //                .ThenInclude(b => b.AnhBaiViets)
        //        .FirstOrDefault(g => g.MaNguoiDung == userId);

        //    var viewModel = new GioHangViewModel();
        //    decimal tongTien = 0m;

        //    if (gioHang != null && gioHang.ChiTietGioHangs.Any())
        //    {
        //        var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
        //        if (string.IsNullOrEmpty(gioHangTamJson))
        //        {
        //            TempData["Error"] = "Giỏ hàng trống!";
        //            return RedirectToAction("Index", "GioHang");
        //        }
        //        var gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(gioHangTamJson);
        //        viewModel.GioHangTam = gioHangTam;
        //        tongTien = gioHangTam.Sum(x => x.GiaSanPham * x.SoLuong);
        //    }
        //    else
        //    {
        //        viewModel.GioHang = gioHang;
        //        tongTien = gioHang.ChiTietGioHangs.Sum(ct => (ct.BaiViet.GiaSanPham ?? 0m) * ct.SoLuong);


        //    }



        public IActionResult Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Account");

            int userId = maNguoiDung.Value;
            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.BaiViet)
                        .ThenInclude(b => b.AnhBaiViets)
                .FirstOrDefault(g => g.MaNguoiDung == userId);

            var viewModel = new GioHangViewModel();
            decimal tongTien = 0m;

            // Kiểm tra nếu giỏ hàng tạm có dữ liệu, lấy từ session
            var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
            if (!string.IsNullOrEmpty(gioHangTamJson))
            {
                var gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(gioHangTamJson);
                viewModel.GioHangTam = gioHangTam;
                tongTien = gioHangTam.Sum(x => x.GiaSanPham * x.SoLuong);
            }
            else if (gioHang != null && gioHang.ChiTietGioHangs.Any())
            {
                viewModel.GioHang = gioHang;
                tongTien = gioHang.ChiTietGioHangs.Sum(ct => (ct.BaiViet.GiaSanPham ?? 0m) * ct.SoLuong);
            }

            // ĐẨY TỔNG TIỀN RA VIEW
            ViewBag.TongTien = tongTien;

            // LẤY VOUCHER ĐÃ LƯU CỦA USER
            //var userVouchers = _context.UserVouchers
            //    .Where(uv => uv.UserId == userId)
            //    .Include(uv => uv.Voucher)
            //    .Where(uv => uv.Voucher != null &&
            //                 uv.Voucher.TrangThai == "HoatDong" &&
            //                 uv.Voucher.NgayBatDau <= DateTime.Now &&
            //                 uv.Voucher.NgayKetThuc >= DateTime.Now)
            //    .Select(uv => uv.Voucher)
            //    .ToList();

            //ViewBag.UserVouchers = userVouchers;
            // LẤY VOUCHER ĐÃ LƯU + CÒN HỢP LỆ (ngày + trạng thái)
            var now = DateTime.Now;
            // TEST: HIỆN TẤT CẢ VOUCHER ĐÃ LƯU (BỎ QUA ĐIỀU KIỆN NGÀY)
            var userVouchers = _context.UserVouchers
                .Where(uv => uv.UserId == userId)
                .Include(uv => uv.Voucher)
                .Where(uv => uv.Voucher != null && uv.Voucher.TrangThai.Trim() == "HoatDong")
                .Select(uv => uv.Voucher)
                .OrderByDescending(v => v.NgayBatDau)
                .ToList();

            ViewBag.UserVouchers = userVouchers;
            return View(viewModel);
        }



        //    // ĐẨY TỔNG TIỀN RA VIEW
        //    ViewBag.TongTien = tongTien;

        //    // LẤY VOUCHER ĐÃ LƯU CỦA USER
        //    var userVouchers = _context.UserVouchers
        //        .Where(uv => uv.UserId == userId)
        //        .Include(uv => uv.Voucher)
        //        .Where(uv => uv.Voucher != null &&
        //                     uv.Voucher.TrangThai == "HoatDong" &&
        //                     uv.Voucher.NgayBatDau <= DateTime.Now &&
        //                     uv.Voucher.NgayKetThuc >= DateTime.Now)
        //        .Select(uv => uv.Voucher)
        //        .ToList();

        //    ViewBag.UserVouchers = userVouchers;

        //    return View(viewModel);
        //}


        // ===============================
        // XÁC NHẬN THANH TOÁN – TẠO ĐƠN HÀNG + HIỆN THÔNG BÁO ĐẸP
        // ===============================
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult XacNhanThanhToan(string diaChiGiaoHang, string phuongThucThanhToan, string phuongThucGiaoHang)
        //{
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (!maNguoiDung.HasValue)
        //        return RedirectToAction("Login", "Account");

        //    int userId = maNguoiDung.Value;

        //    // Kiểm tra giỏ hàng tạm trong session
        //    List<GioHangTamModel> gioHangTam = null;

        //    if (HttpContext.Session.TryGetValue("GioHangTam", out var gioHangTamJson))
        //    {
        //        // Deserialize giỏ hàng tạm
        //        gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(Encoding.UTF8.GetString(gioHangTamJson));
        //    }

        //    if (gioHangTam == null || !gioHangTam.Any())
        //    {
        //        TempData["Error"] = "Giỏ hàng của bạn đang trống!";
        //        return RedirectToAction("Index", "GioHang");
        //    }

        //    try
        //    {
        //        // Tính tổng tiền của giỏ hàng tạm
        //        decimal tongTienNhom = gioHangTam.Sum(ct => ct.GiaSanPham * ct.SoLuong);

        //        // Tạo đơn hàng cho mỗi người bán
        //        foreach (var nhom in gioHangTam.GroupBy(item => item.MaNguoiBan))
        //        {
        //            var donHang = new DonHang
        //            {
        //                MaNguoiMua = userId,
        //                MaNguoiBan = nhom.Key,  // Lấy MaNguoiBan từ nhóm sản phẩm
        //                DiaChiGiaoHang = diaChiGiaoHang,
        //                PhuongThucThanhToan = phuongThucThanhToan,
        //                PhuongThucGiaoHang = phuongThucGiaoHang,
        //                TrangThai = "Chờ xác nhận",
        //                NgayDat = DateTime.Now,
        //                TongTien = nhom.Sum(item => item.GiaSanPham * item.SoLuong)
        //            };

        //            // Lưu đơn hàng vào cơ sở dữ liệu
        //            _context.DonHangs.Add(donHang);
        //            _context.SaveChanges();

        //            // Tạo chi tiết đơn hàng
        //            foreach (var item in nhom)
        //            {
        //                var chiTiet = new ChiTietDonHang
        //                {
        //                    MaDonHang = donHang.MaDonHang,
        //                    MaBaiViet = item.MaBaiViet,
        //                    SoLuong = item.SoLuong,
        //                    Gia = item.GiaSanPham
        //                };

        //                _context.ChiTietDonHangs.Add(chiTiet);

        //            }

        //            _context.SaveChanges();
        //        }

        //        // Xóa giỏ hàng tạm khỏi session sau khi thanh toán
        //        HttpContext.Session.Remove("GioHangTam");

        //        TempData["Success"] = "Đặt hàng thành công! Đơn hàng đang chờ người bán xác nhận.";
        //        return View("ThongBao");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Có lỗi xảy ra khi đặt hàng.");
        //        TempData["Error"] = "Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại!";
        //        return RedirectToAction("Index");
        //    }
        //}
        //----------------------------------------------------------------------------------------------------------
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult XacNhanThanhToan(string diaChiGiaoHang, string phuongThucThanhToan, string phuongThucGiaoHang)
        //{
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (!maNguoiDung.HasValue)
        //        return RedirectToAction("Login", "Account");

        //    int userId = maNguoiDung.Value;

        //    // Kiểm tra giỏ hàng tạm trong session
        //    List<GioHangTamModel> gioHangTam = null;

        //    if (HttpContext.Session.TryGetValue("GioHangTam", out var gioHangTamJson))
        //    {
        //        // Deserialize giỏ hàng tạm
        //        gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(Encoding.UTF8.GetString(gioHangTamJson));
        //    }

        //    if (gioHangTam == null || !gioHangTam.Any())
        //    {
        //        TempData["Error"] = "Giỏ hàng của bạn đang trống!";
        //        return RedirectToAction("Index", "GioHang");
        //    }

        //    try
        //    {
        //        // Tính tổng tiền của giỏ hàng tạm
        //        decimal tongTienNhom = gioHangTam.Sum(ct => ct.GiaSanPham * ct.SoLuong);

        //        // Tạo đơn hàng cho mỗi người bán
        //        foreach (var nhom in gioHangTam.GroupBy(item => item.MaNguoiBan))
        //        {
        //            var donHang = new DonHang
        //            {
        //                MaNguoiMua = userId,
        //                MaNguoiBan = nhom.Key,  // Lấy MaNguoiBan từ nhóm sản phẩm
        //                DiaChiGiaoHang = diaChiGiaoHang,
        //                PhuongThucThanhToan = phuongThucThanhToan,
        //                PhuongThucGiaoHang = phuongThucGiaoHang,
        //                TrangThai = "Chờ xác nhận",
        //                NgayDat = DateTime.Now,
        //                TongTien = nhom.Sum(item => item.GiaSanPham * item.SoLuong)
        //            };

        //            // Lưu đơn hàng vào cơ sở dữ liệu
        //            _context.DonHangs.Add(donHang);
        //            _context.SaveChanges();

        //            // Tạo chi tiết đơn hàng và giảm số lượng sản phẩm
        //            foreach (var item in nhom)
        //            {
        //                var chiTiet = new ChiTietDonHang
        //                {
        //                    MaDonHang = donHang.MaDonHang,
        //                    MaBaiViet = item.MaBaiViet,
        //                    SoLuong = item.SoLuong,
        //                    Gia = item.GiaSanPham
        //                };

        //                _context.ChiTietDonHangs.Add(chiTiet);

        //                // Cập nhật số lượng sản phẩm trong bảng BaiViet
        //                var sanPham = _context.BaiViets.Find(item.MaBaiViet);
        //                if (sanPham != null)
        //                {
        //                    // Giảm số lượng sản phẩm theo số lượng trong giỏ hàng
        //                    sanPham.SoLuong -= item.SoLuong;
        //                    _context.BaiViets.Update(sanPham); // Cập nhật lại số lượng
        //                }
        //            }

        //            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        //        }

        //        // Xóa giỏ hàng tạm khỏi session sau khi thanh toán
        //        HttpContext.Session.Remove("GioHangTam");

        //        TempData["Success"] = "Đặt hàng thành công! Đơn hàng đang chờ người bán xác nhận.";
        //        return View("ThongBao");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Có lỗi xảy ra khi đặt hàng.");
        //        TempData["Error"] = "Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại!";
        //        return RedirectToAction("Index");
        //    }
        //}
        //------------------------------------------------------------root
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult XacNhanThanhToan(string diaChiGiaoHang, string phuongThucThanhToan, string phuongThucGiaoHang, decimal tongTien)
        //{
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (!maNguoiDung.HasValue)
        //        return RedirectToAction("Login", "Account");

        //    int userId = maNguoiDung.Value;

        //    // Kiểm tra giỏ hàng tạm trong session
        //    List<GioHangTamModel> gioHangTam = null;

        //    if (HttpContext.Session.TryGetValue("GioHangTam", out var gioHangTamJson))
        //    {
        //        // Deserialize giỏ hàng tạm
        //        gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(Encoding.UTF8.GetString(gioHangTamJson));
        //    }

        //    if (gioHangTam == null || !gioHangTam.Any())
        //    {
        //        TempData["Error"] = "Giỏ hàng của bạn đang trống!";
        //        return RedirectToAction("Index", "GioHang");
        //    }

        //    // Lấy voucher đã lưu của người dùng
        //    var voucherId = HttpContext.Request.Form["voucherId"]; // Lấy voucherId từ form
        //    decimal discount = 0;

        //    if (!string.IsNullOrEmpty(voucherId))
        //    {
        //        var voucher = _context.UuDais
        //            .FirstOrDefault(v => v.MaUuDai == int.Parse(voucherId) && v.TrangThai == "HoatDong" && v.NgayBatDau <= DateTime.Now && v.NgayKetThuc >= DateTime.Now);

        //        if (voucher != null)
        //        {
        //            if (voucher.LoaiUuDai == "PhanTram")
        //            {
        //                discount = (tongTien * voucher.GiaTri) / 100; // Áp dụng giảm giá phần trăm
        //            }
        //            else if (voucher.LoaiUuDai == "TienMat")
        //            {
        //                discount = voucher.GiaTri; // Áp dụng giảm giá tiền mặt
        //            }
        //        }
        //    }

        //    // Tính tổng tiền sau khi áp dụng voucher
        //    decimal tongTienSauKhiApDung = tongTien - discount;

        //    // Tạo đơn hàng cho mỗi người bán
        //    foreach (var nhom in gioHangTam.GroupBy(item => item.MaNguoiBan))
        //    {
        //        var donHang = new DonHang
        //        {
        //            MaNguoiMua = userId,
        //            MaNguoiBan = nhom.Key,
        //            DiaChiGiaoHang = diaChiGiaoHang,
        //            PhuongThucThanhToan = phuongThucThanhToan,
        //            PhuongThucGiaoHang = phuongThucGiaoHang,
        //            TrangThai = "Chờ xác nhận",
        //            NgayDat = DateTime.Now,
        //            TongTien = nhom.Sum(item => item.GiaSanPham * item.SoLuong) - discount // Áp dụng discount vào tổng tiền
        //        };

        //        _context.DonHangs.Add(donHang);
        //        _context.SaveChanges();

        //        // Tạo chi tiết đơn hàng
        //        foreach (var item in nhom)
        //        {
        //            var chiTiet = new ChiTietDonHang
        //            {
        //                MaDonHang = donHang.MaDonHang,
        //                MaBaiViet = item.MaBaiViet,
        //                SoLuong = item.SoLuong,
        //                Gia = item.GiaSanPham
        //            };

        //            _context.ChiTietDonHangs.Add(chiTiet);
        //        }

        //        _context.SaveChanges();
        //    }

        //    // Xóa giỏ hàng tạm khỏi session sau khi thanh toán
        //    HttpContext.Session.Remove("GioHangTam");

        //    TempData["Success"] = "Đặt hàng thành công! Đơn hàng đang chờ người bán xác nhận.";
        //    return View("ThongBao");
        //}

        //---------------------------test
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult XacNhanThanhToan(string diaChiGiaoHang, string phuongThucThanhToan, string phuongThucGiaoHang, decimal tongTien, string voucherId)
        //{
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (!maNguoiDung.HasValue)
        //        return RedirectToAction("Login", "Account"); // ĐÃ SỬA: thêm dấu ", đóng ngoặc đúng

        //    int userId = maNguoiDung.Value;

        //    var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
        //    var gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(gioHangTamJson);
        //    if (string.IsNullOrEmpty(gioHangTamJson))
        //    {
        //        TempData["Error"] = "Giỏ hàng trống!";
        //        return RedirectToAction("Index", "GioHang");
        //    }
        //    else
        //     {

        //        if (gioHangTam == null || !gioHangTam.Any())
        //        {
        //            TempData["Error"] = "Giỏ hàng trống!";
        //            return RedirectToAction("Index", "GioHang");
        //        }

        //    }




        //    decimal discount = 0;
        //    int? selectedVoucherId = null;

        //    // LẤY VOUCHER TỪ FORM (string → parse thành int)
        //    if (!string.IsNullOrEmpty(voucherId) && int.TryParse(voucherId, out int vid))
        //    {
        //        selectedVoucherId = vid;
        //        //var voucher = _context.UuDais.FirstOrDefault(v => v.MaUuDai == vid);
        //        //if (voucher != null)
        //        //{
        //        //    if (voucher.LoaiUuDai == "PhanTram")
        //        //        discount = tongTien * voucher.GiaTri / 100;
        //        //    else if (voucher.LoaiUuDai == "TienMat")
        //        //        discount = voucher.GiaTri;
        //        //}
        //    }

        //    try
        //    {
        //        foreach (var nhom in gioHangTam.GroupBy(item => item.MaNguoiBan))
        //        {
        //            var donHang = new DonHang
        //            {
        //                MaNguoiMua = userId,
        //                MaNguoiBan = nhom.Key,
        //                DiaChiGiaoHang = diaChiGiaoHang,
        //                PhuongThucThanhToan = phuongThucThanhToan,
        //                PhuongThucGiaoHang = phuongThucGiaoHang,
        //                TrangThai = "Chờ xác nhận",
        //                NgayDat = DateTime.Now,
        //                TongTien = nhom.Sum(x => x.GiaSanPham * x.SoLuong)// - discount
        //            };

        //            _context.DonHangs.Add(donHang);
        //            _context.SaveChanges();

        //            foreach (var item in nhom)
        //            {
        //                _context.ChiTietDonHangs.Add(new ChiTietDonHang
        //                {
        //                    MaDonHang = donHang.MaDonHang,
        //                    MaBaiViet = item.MaBaiViet,
        //                    SoLuong = item.SoLuong,
        //                    Gia = item.GiaSanPham
        //                });
        //            }
        //            _context.SaveChanges();
        //            // TRỪ SỐ LƯỢNG SẢN PHẨM TRONG BẢI VIẾT
        //            foreach (var item in nhom)
        //            {
        //                var sanPham = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == item.MaBaiViet);
        //                if (sanPham != null && sanPham.SoLuong >= item.SoLuong)
        //                {
        //                    sanPham.SoLuong -= item.SoLuong;
        //                }
        //            }
        //            _context.SaveChanges();
        //        }

        //       // XÓA VOUCHER ĐÃ DÙNG(CHỈ DÙNG 1 LẦN)
        //        if (selectedVoucherId.HasValue)
        //        {
        //            var usedVoucher = _context.UserVouchers
        //                .FirstOrDefault(uv => uv.UserId == userId && uv.VoucherId == selectedVoucherId.Value);

        //            if (usedVoucher != null)
        //            {
        //                _context.UserVouchers.Remove(usedVoucher);
        //                _context.SaveChanges();
        //            }
        //        }

        //        HttpContext.Session.Remove("GioHangTam");
        //        TempData["Success"] = "Đặt hàng thành công! Voucher đã được sử dụng và xóa khỏi ví.";
        //        return View("ThongBao");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Lỗi khi đặt hàng");
        //        TempData["Error"] = "Có lỗi xảy ra khi đặt hàng! Vui lòng thử lại.";
        //        return RedirectToAction("Index");
        //    }
        //}


      
        public IActionResult XacNhanThanhToan(string diaChiGiaoHang, string phuongThucThanhToan, string phuongThucGiaoHang, decimal tongTien, string voucherId)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Account");

            int userId = maNguoiDung.Value;

            var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
            var gioHangTam = string.IsNullOrEmpty(gioHangTamJson)
                ? null
                : JsonConvert.DeserializeObject<List<GioHangTamModel>>(gioHangTamJson);

            // If gioHangTam is empty, retrieve products from gioHangs table
            bool isFromGioHang = false; // Flag to check if the order is from the main cart or session cart
            if (gioHangTam == null || !gioHangTam.Any())
            {
                var gioHang = _context.GioHangs
                    .Include(g => g.ChiTietGioHangs)
                        .ThenInclude(ct => ct.BaiViet)
                    .FirstOrDefault(g => g.MaNguoiDung == userId);

                if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                {
                    TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                    return RedirectToAction("Index", "GioHang");
                }
                if (gioHang != null)
                {
                    isFromGioHang = true;
                }

                gioHangTam = gioHang.ChiTietGioHangs.Select(ct => new GioHangTamModel
                {
                    MaBaiViet = ct.MaBaiViet,
                    TenSanPham = ct.BaiViet.TieuDe,
                    GiaSanPham = ct.BaiViet.GiaSanPham ?? 0,
                    SoLuong = ct.SoLuong,
                    MaNguoiBan = ct.BaiViet.MaNguoiDung,
                    MaNguoiMua = userId,
                    AnhSanPham = ct.BaiViet.AnhBaiViets?.FirstOrDefault()?.DuongDan ?? "/images/no-image.png"
                }).ToList();
            }

            //decimal discount = 0;
            int? selectedVoucherId = null;

            // Validate and apply voucher
            if (!string.IsNullOrEmpty(voucherId) && int.TryParse(voucherId, out int vid))
            {
                selectedVoucherId = vid;
               
            }
            Debug.WriteLine($"Tổng tiền sau khi giảm giá: {tongTien}");

            try
            {
               

                // Create the order for each seller
                foreach (var nhom in gioHangTam.GroupBy(item => item.MaNguoiBan))
                {
                    var donHang = new DonHang
                    {
                        MaNguoiMua = userId,
                        MaNguoiBan = nhom.Key,
                        DiaChiGiaoHang = diaChiGiaoHang,
                        PhuongThucThanhToan = phuongThucThanhToan,
                        PhuongThucGiaoHang = phuongThucGiaoHang,
                        TrangThai = "Chờ xác nhận",
                        NgayDat = DateTime.Now,
                        TongTien = tongTien// - discount // Apply discount
                    };

                    _context.DonHangs.Add(donHang);
                    _context.SaveChanges(); // Save the order

                    // Create order details for each item in the cart
                    foreach (var item in nhom)
                    {
                        _context.ChiTietDonHangs.Add(new ChiTietDonHang
                        {
                            MaDonHang = donHang.MaDonHang,
                            MaBaiViet = item.MaBaiViet,
                            SoLuong = item.SoLuong,
                            Gia = item.GiaSanPham
                        });
                    }

                    _context.SaveChanges(); // Save the order details

                    // Update product stock after order is placed
                    foreach (var item in nhom)
                    {
                        var sanPham = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == item.MaBaiViet);
                        if (sanPham != null && sanPham.SoLuong >= item.SoLuong)
                        {
                            sanPham.SoLuong -= item.SoLuong; // Decrease stock by the quantity ordered
                            _context.BaiViets.Update(sanPham); // Save stock update
                        }
                        else
                        {
                            TempData["Error"] = $"Sản phẩm {sanPham?.TieuDe} không đủ số lượng!";
                            return RedirectToAction("Index", "GioHang");
                        }
                    }
                    _context.SaveChanges(); // Final save for all stock updates
                }

                // Remove used voucher from the user's voucher list
                if (selectedVoucherId.HasValue)
                {
                    var usedVoucher = _context.UserVouchers
                        .FirstOrDefault(uv => uv.UserId == userId && uv.VoucherId == selectedVoucherId.Value);

                    if (usedVoucher != null)
                    {
                        _context.UserVouchers.Remove(usedVoucher);
                        _context.SaveChanges();
                    }
                }

                // Clear the session after successful order placement
                HttpContext.Session.Remove("GioHangTam");

                // Delete the GioHang from the database **only if the order is created from the main cart (gioHang)**
                if (isFromGioHang)
                {
                    var gioHang = _context.GioHangs.FirstOrDefault(g => g.MaNguoiDung == userId);
                    if (gioHang != null)
                    {
                        _context.GioHangs.Remove(gioHang);
                        _context.SaveChanges(); // Commit the removal of the main cart
                    }
                }

                TempData["Success"] = "Đặt hàng thành công! Voucher đã được sử dụng và xóa khỏi ví.";
                return View("ThongBao");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đặt hàng");
                TempData["Error"] = "Có lỗi xảy ra khi đặt hàng! Vui lòng thử lại.";
                return RedirectToAction("Index");
            }
        }






        public IActionResult MuaNgay(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Account");

            var sanPham = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Include(b => b.NguoiDung)
                .FirstOrDefault(b => b.MaBaiViet == id && b.SoLuong > 0);

            if (sanPham == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại hoặc đã hết!";
                return RedirectToAction("Index", "Home");
            }

            // Tạo giỏ hàng tạm với sản phẩm được chọn
            var gioHangTam = new List<GioHangTamModel>
    {
        new GioHangTamModel
        {
            MaBaiViet = sanPham.MaBaiViet,
            TenSanPham = sanPham.TieuDe,
            GiaSanPham = sanPham.GiaSanPham ?? 0,
            SoLuong = 1,
            MaNguoiBan = sanPham.MaNguoiDung,
            MaNguoiMua= maNguoiDung.Value,

           AnhSanPham = sanPham.AnhBaiViets?.FirstOrDefault()?.DuongDan
              ?? "/images/no-image.png"
        }
    };

            // Lưu giỏ hàng tạm vào session dưới dạng JSON
            HttpContext.Session.SetString("GioHangTam", JsonConvert.SerializeObject(gioHangTam));
                    

            // Kiểm tra lại dữ liệu trong session
            var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
            if (string.IsNullOrEmpty(gioHangTamJson))
            {
                TempData["Error"] = "Không tìm thấy giỏ hàng tạm!";
                return RedirectToAction("Index", "Home");
            }

            // Chuyển người dùng đến trang thanh toán với giỏ hàng tạm
            return RedirectToAction("Index", "ThanhToan");


            // Chuyển người dùng đến trang thanh toán với giỏ hàng tạm
            return RedirectToAction("Index", "ThanhToan");
        }


        // Action cho trang thông báo sau khi thanh toán
        public IActionResult ThongBao()
        {
            return View();
        }

    }
}
