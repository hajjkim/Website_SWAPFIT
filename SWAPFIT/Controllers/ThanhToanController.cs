using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Data;
using SWAPFIT.Model;
using SWAPFIT.Models;

namespace SWAPFIT.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThanhToanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // 🟢 Trang xác nhận thanh toán
        // ===============================
        public IActionResult Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.BaiViet)
                        .ThenInclude(b => b.AnhBaiViets)
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.BaiViet)
                        .ThenInclude(b => b.NguoiDung)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "GioHang");
            }

            // 🔹 Bơm danh sách voucher đã lưu
            ViewBag.UserVouchers = LayVoucherDaLuu(maNguoiDung.Value);

            return View(gioHang);
        }




        // ===========================================================
        // 🟢 Xác nhận ĐẶT HÀNG cho MUA NGAY
        // ===========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhanMuaNgay(int id, string diaChiGiaoHang,
     string phuongThucThanhToan, string phuongThucGiaoHang)
        {
            // Lấy mã người dùng từ session
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            // Lấy thông tin sản phẩm từ database
            var sanPham = await _context.BaiViets
                .Include(b => b.NguoiDung)
                .FirstOrDefaultAsync(b => b.MaBaiViet == id);

            // Nếu không tìm thấy sản phẩm, chuyển hướng về trang ChoTang
            if (sanPham == null) return RedirectToAction("ChoTang", "TinTuc");

            // Tạo đơn hàng mới
            var donHang = new DonHang
            {
                MaNguoiMua = maNguoiDung.Value, // Gán mã người mua từ session
                MaNguoiBan = sanPham.MaNguoiDung, // Gán mã người bán từ sản phẩm
                DiaChiGiaoHang = diaChiGiaoHang,
                PhuongThucThanhToan = phuongThucThanhToan,
                PhuongThucGiaoHang = phuongThucGiaoHang,
                TrangThai = "Chờ xác nhận", // Đơn hàng đang chờ xử lý
                NgayDat = DateTime.Now,
                TongTien = sanPham.GiaSanPham ?? 0
            };

            // Thêm đơn hàng vào cơ sở dữ liệu
            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync(); // Lưu để có MaDonHang

            // Lưu chi tiết đơn hàng
            _context.ChiTietDonHangs.Add(new ChiTietDonHang
            {
                MaDonHang = donHang.MaDonHang, // Gán mã đơn hàng
                MaBaiViet = sanPham.MaBaiViet, // Gán mã sản phẩm
                SoLuong = 1, // Số lượng sản phẩm trong đơn
                Gia = sanPham.GiaSanPham ?? 0 // Gán giá sản phẩm
            });

            // Trừ số lượng sản phẩm trong kho
            sanPham.SoLuong -= 1;

            if (sanPham.SoLuong <= 0)
            {
                sanPham.TrangThai = "Hết hàng"; // Đặt trạng thái sản phẩm là "Hết hàng"
            }

            // Lưu thay đổi
            await _context.SaveChangesAsync();

            // Thông báo thành công
            TempData["DaDatHang"] = true;
            TempData["Success"] = "Đặt hàng thành công!";
            TempData["ThongBao"] = "Đơn hàng của bạn đã được đặt thành công và đang chờ người bán xác nhận!";

            return RedirectToAction("ThongBao"); // Chuyển hướng tới trang thông báo
        }



        // ===========================================================
        // 🟢 MUA NGAY
        // ===========================================================
        public IActionResult MuaNgay(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            var sanPham = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Include(b => b.NguoiDung)
                .FirstOrDefault(b => b.MaBaiViet == id);

            if (sanPham == null) return NotFound();

            var gioHangTam = new GioHang
            {
                ChiTietGioHangs = new List<ChiTietGioHang>
        {
            new ChiTietGioHang
            {
                BaiViet = sanPham,
                SoLuong = 1
            }
        }
            };

            // 🔹 Bơm danh sách voucher đã lưu cho view dùng dropdown
            ViewBag.UserVouchers = LayVoucherDaLuu(maNguoiDung.Value);

            return View("Index", gioHangTam);
        }


        // ===========================================================
        // 🟢 Trang Thông báo sau khi đặt hàng
        // ===========================================================
        // Trang Thông báo sau khi đặt hàng
        public IActionResult ThongBao()
        {
            TempData["DaDatHang"] = true;
            TempData.Keep("DaDatHang");
            ViewBag.Message = "Đơn hàng của bạn đã được đặt thành công và đang chờ người bán xác nhận!";
            return View(); // Đảm bảo rằng ThongBao.cshtml tồn tại trong thư mục Views/ThanhToan
        }

        private List<UserVoucher> LayVoucherDaLuu(int maNguoiDung)
        {
            return _context.UserVouchers
                .Include(uv => uv.Voucher) // UuDai
                .Where(uv => uv.UserId == maNguoiDung
                             && uv.Voucher.NgayBatDau <= DateTime.Today
                             && uv.Voucher.NgayKetThuc >= DateTime.Today
                             && uv.Voucher.TrangThai == "HoatDong")
                .OrderBy(uv => uv.Voucher.NgayKetThuc)
                .ToList();
        }

    }
}
