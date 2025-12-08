using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Data;
using SWAPFIT.Models;

namespace SWAPFIT.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // 🛒 Người mua: Lịch sử đơn hàng
        // ===============================
        public IActionResult LichSuDonHang()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            var donHangs = _context.DonHangs
                .Where(d => d.MaNguoiMua == maNguoiDung)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BaiViet)
                .Include(d => d.NguoiBan)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(donHangs);
        }

        // ===============================
        // 🧾 Chi tiết đơn hàng (người bán xem)
        // ===============================
        public IActionResult ChiTietDonHang(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            var donHang = _context.DonHangs
                .Where(d => d.MaDonHang == id && d.MaNguoiBan == maNguoiDung)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BaiViet)
                    .ThenInclude(bv => bv.AnhBaiViets)
                .Include(d => d.NguoiMua)
                .FirstOrDefault();

            if (donHang == null) return NotFound();

            return View(donHang);  // Ensure you're passing DonHang model
        }



        // ===============================
        // 🛍️ Người mua: Đơn tôi đã mua
        // ===============================
        public IActionResult DonHangToiDaMua()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            var donHangs = _context.DonHangs
                .Where(d => d.MaNguoiMua == maNguoiDung)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BaiViet)
                .Include(d => d.NguoiBan)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(donHangs);
        }

        // ===============================
        // 🛍️ Người bán: Đơn người khác mua từ tôi
        // ===============================
        public IActionResult DonHangNguoiKhacMuaTuToi()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            var donHangs = _context.DonHangs
                .Where(d => d.MaNguoiBan == maNguoiDung)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BaiViet)
                .Include(d => d.NguoiMua)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(donHangs);
        }

        // ===============================
        // ⚙️ Người bán: Cập nhật trạng thái đơn hàng
        // ===============================
        [HttpPost]
        public IActionResult CapNhatTrangThai(int id, string trangThai)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            var donHang = _context.DonHangs.FirstOrDefault(d => d.MaDonHang == id);
            if (donHang == null) return NotFound();

            // Chỉ người bán mới cập nhật được
            if (donHang.MaNguoiBan != maNguoiDung)
                return Forbid();

            donHang.TrangThai = trangThai;
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật trạng thái thành công!";
            return RedirectToAction("ChiTietDonHang", new { id });
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HuyDonHangNguoiMua(int id, string? lyDoHuy)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
                return RedirectToAction("Login", "Account");

            // Tìm đơn của chính người mua
            var donHang = _context.DonHangs
                .Include(d => d.ChiTietDonHangs).ThenInclude(ct => ct.BaiViet)
                .Include(d => d.NguoiBan)
                .FirstOrDefault(d => d.MaDonHang == id && d.MaNguoiMua == maNguoiDung);

            if (donHang == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("LichSuDonHang");
            }

            // Không cho hủy nếu đã hoàn thành / đã giao
            if (donHang.TrangThai == "HoanThanh" || donHang.TrangThai == "DaGiao")
            {
                TempData["Error"] = "Đơn hàng đã được xác nhận giao hoặc hoàn thành, không thể hủy.";
                return RedirectToAction("LichSuDonHang");
            }

            // Nếu đã hủy rồi thì thôi
            if (donHang.TrangThai == "DaHuy")
            {
                TempData["Error"] = "Đơn hàng đã được hủy trước đó.";
                return RedirectToAction("LichSuDonHang");
            }

            // Cập nhật trạng thái + lý do
            donHang.TrangThai = "DaHuy";
            donHang.LyDoHuy = lyDoHuy;

            // Gửi thông báo cho người bán
            var baiVietDauTien = donHang.ChiTietDonHangs.FirstOrDefault()?.BaiViet;
            var maNguoiBan = baiVietDauTien?.MaNguoiDung ?? donHang.MaNguoiBan;

            if (maNguoiBan != null)
            {
                var tb = new ThongBao
                {
                    MaNguoiDung = maNguoiBan.Value,
                    NoiDung = $"Đơn hàng #{donHang.MaDonHang} đã bị người mua hủy. Lý do: {lyDoHuy}",
                    LienKet = Url.Action("ChiTietDonHang", "DonHang",
                                         new { id = donHang.MaDonHang }, Request.Scheme),
                    DaXem = false,
                    NgayTao = DateTime.Now
                };
                _context.ThongBaos.Add(tb);
            }

            _context.SaveChanges();

            TempData["Success"] = "Bạn đã hủy đơn hàng thành công.";
            return RedirectToAction("LichSuDonHang");
        }
        


    }
}
