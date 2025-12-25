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
        // 🧾 Chi tiết đơn hàng
        // ===============================
        //public IActionResult ChiTietDonHang(int id)
        //{
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (maNguoiDung == null) return RedirectToAction("Login", "Account");

        //    var donHang = _context.DonHangs
        //        .Where(d => d.MaDonHang == id && d.MaNguoiBan == maNguoiDung)
        //        .Include(d => d.ChiTietDonHangs)
        //            .ThenInclude(ct => ct.BaiViet)
        //        .Include(d => d.NguoiMua)
        //        .FirstOrDefault();

        //    if (donHang == null) return NotFound();

        //    return View(donHang);
        //}

        public IActionResult ChiTietDonHang(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
                return RedirectToAction("Login", "Account");

            // Lấy chi tiết đơn hàng
            var donHang = _context.DonHangs
                .Where(d => d.MaDonHang == id && d.MaNguoiBan == maNguoiDung) // Đảm bảo chỉ người bán mới xem được
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BaiViet)
                     .ThenInclude(bv => bv.AnhBaiViets)
                .Include(d => d.NguoiMua)
                .FirstOrDefault();

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }


        // ===============================
        // 🛍️ Người bán: Danh sách đơn hàng của tôi
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
            return RedirectToAction("ChiTietDonHang", new { id = id });
        }
    }
}
