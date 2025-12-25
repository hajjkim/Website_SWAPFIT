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

       

        public IActionResult ChiTietDonHang(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Account");

            int sellerId = maNguoiDung.Value;

            var donHang = _context.DonHangs
                .Where(d => d.MaDonHang == id && d.MaNguoiBan == sellerId)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BaiViet)
                        .ThenInclude(bv => bv.AnhBaiViets)
                .Include(d => d.NguoiMua)
               
                .FirstOrDefault();

            if (donHang == null) return NotFound();

            return View(donHang);
        }


     
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatTrangThai(int id, string trangThai)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            var donHang = _context.DonHangs.FirstOrDefault(d => d.MaDonHang == id);
            if (donHang == null) return NotFound();

            if (donHang.MaNguoiBan != maNguoiDung.Value)
                return Forbid();

            donHang.TrangThai = trangThai;
            if (!donHang.MaNguoiMua.HasValue)
            {
                TempData["Error"] = "Không xác định được người mua để gửi thông báo.";
                return RedirectToAction("ChiTietDonHang", new { id = id });
            }

            _context.ThongBaos.Add(new ThongBao
            {
                MaNguoiDung = donHang.MaNguoiMua.Value,
                NoiDung = $"Đơn #{donHang.MaDonHang} đã được cập nhật trạng thái: {trangThai}.",
                LienKet = Url.Action("DonHangToiDaMua", "DonHang"),
                DaXem = false,
                NgayTao = DateTime.Now,
                Loai = "DonHang",
                ThamChieuId = donHang.MaDonHang
            });

            _context.SaveChanges();

            TempData["Success"] = "Cập nhật trạng thái thành công!";
            return RedirectToAction("ChiTietDonHang", new { id = id });
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HuyDonHangNguoiMua(int id, string lyDoHuy)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null) return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(lyDoHuy))
            {
                TempData["Error"] = "Vui lòng nhập lý do hủy.";
                return RedirectToAction("DonHangToiDaMua");
            }

            var donHang = _context.DonHangs
                .FirstOrDefault(d => d.MaDonHang == id && d.MaNguoiMua == maNguoiDung.Value);

            if (donHang == null) return NotFound();

            var tt = (donHang.TrangThai ?? "").Trim();
            if (!string.Equals(tt, "Chờ xác nhận", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Chỉ có thể hủy đơn khi đơn đang ở trạng thái 'Chờ xác nhận'.";
                return RedirectToAction("DonHangToiDaMua");
            }

            donHang.TrangThai = "Đã hủy";

            if (!donHang.MaNguoiBan.HasValue)
            {
                _context.SaveChanges();
                TempData["Error"] = "Không xác định được người bán để gửi thông báo.";
                return RedirectToAction("DonHangToiDaMua");
            }

            _context.ThongBaos.Add(new ThongBao
            {
                MaNguoiDung = donHang.MaNguoiBan.Value,
                NoiDung = $"Đơn #{donHang.MaDonHang} đã bị người mua hủy. Lý do: {lyDoHuy}",
                LienKet = Url.Action("ChiTietDonHang", "DonHang", new { id = donHang.MaDonHang }),
                DaXem = false,
                NgayTao = DateTime.Now,
                Loai = "DonHang",
                ThamChieuId = donHang.MaDonHang
            });

            _context.SaveChanges();

            TempData["Success"] = "Đã hủy đơn và đã thông báo cho người bán!";
            return RedirectToAction("DonHangToiDaMua");
        }



    }
}
