using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Data;
using SWAPFIT.Models;
<<<<<<< HEAD
using SWAPFIT.Model;   // nếu UserVoucher nằm trong namespace này
using System;
=======
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
using System.Linq;

namespace SWAPFIT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
        // ================== TRANG CHỦ ==================
        //public IActionResult Index()
        //{
        //    // 1️⃣ Lấy danh mục (model chính của view)
        //    var danhMucs = _context.DanhMucs.ToList();

        //    // 2️⃣ Lấy danh sách voucher đang hoạt động
        //    var now = DateTime.Now;

        //    var activeVouchers = _context.UuDais
        //        .Where(v =>
        //            v.TrangThai == "HoatDong" &&
        //            v.NgayBatDau <= now &&
        //            v.NgayKetThuc >= now
        //        )
        //        .ToList();

        //    // Nếu có giới hạn số lượng thì lọc thêm
        //    activeVouchers = activeVouchers
        //        .Where(v =>
        //            !v.GioiHanSoLuong.HasValue ||    // không giới hạn
        //            _context.UserVouchers.Count(uv => uv.VoucherId == v.MaUuDai)
        //                < v.GioiHanSoLuong.Value     // còn slot
        //        )
        //        .OrderByDescending(v => v.NgayBatDau)
        //        .ToList();

        //    // 3️⃣ Đẩy sang ViewBag để view dùng
        //    ViewBag.ActiveVouchers = activeVouchers;

        //    return View(danhMucs);
        //}
        //----------------------------------------
        public IActionResult Index()
        {
            var danhMucs = _context.DanhMucs.ToList();
            var now = DateTime.Now;

            var activeVouchers = _context.UuDais
                .Where(v =>
                    v.TrangThai.Trim() == "HoatDong" &&
                    v.NgayBatDau <= DateTime.Now.AddMonths(12) &&  // cho phép voucher trong tương lai 12 tháng
                    v.NgayKetThuc >= DateTime.Now.AddMonths(-12))   // chấp nhận cả quá khứ
                .ToList();

            // Lọc giới hạn số lượng (chỉ 1 lần query DB – siêu nhanh)
            var claimedCounts = _context.UserVouchers
                .GroupBy(uv => uv.VoucherId)
                .Select(g => new { VoucherId = g.Key, Count = g.Count() })
                .ToDictionary(x => x.VoucherId, x => x.Count);

            activeVouchers = activeVouchers
                .Where(v =>
                    !v.GioiHanSoLuong.HasValue ||
                    !claimedCounts.ContainsKey(v.MaUuDai) ||
                    claimedCounts[v.MaUuDai] < v.GioiHanSoLuong.Value)
                .OrderByDescending(v => v.NgayBatDau)
                .ToList();

=======
        // Hiển thị danh mục
        public IActionResult Index()
        {
            // Fetching active vouchers that are still valid
            var activeVouchers = _context.UuDais
                                        .Where(u => u.TrangThai == "HoatDong" && u.NgayKetThuc >= DateTime.Now)
                                        .ToList();

            // Fetching other data you need for the page
            var danhMucs = _context.DanhMucs.ToList();

            // Passing the data to the view
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            ViewBag.ActiveVouchers = activeVouchers;

            return View(danhMucs);
        }

<<<<<<< HEAD

        // =============== THÊM DANH MỤC ===============
        [HttpPost]
        public IActionResult ThemDanhMuc(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                _context.DanhMucs.Add(danhMuc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var danhMucs = _context.DanhMucs.ToList();
            return View("Index", danhMucs);
=======

        // Thêm danh mục mới
        [HttpPost]
        public IActionResult ThemDanhMuc(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                _context.DanhMucs.Add(danhMuc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var danhMucs = _context.DanhMucs.ToList();
            return View("Index", danhMucs);
        }

        // Action xử lý tìm kiếm
        public IActionResult Search(string query)
        {
            // Kiểm tra nếu query rỗng
            if (string.IsNullOrEmpty(query))
            {
                // Nếu không có query, hiển thị toàn bộ danh mục
                var allDanhMucs = _context.DanhMucs.ToList();
                return View("Index", allDanhMucs);
            }

            // Nếu có query, tìm kiếm theo tên danh mục
            var searchResults = _context.DanhMucs
                .Where(d => d.TenDanhMuc.Contains(query))  // Giả sử bạn tìm kiếm theo thuộc tính TenDanhMuc
                .ToList();

            return View("Index", searchResults); // Trả về kết quả tìm kiếm
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        }
        public IActionResult VoucherDetails(int id)
        {
            var voucher = _context.UuDais.FirstOrDefault(u => u.MaUuDai == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher); // Pass the voucher details to the view
        }

    }
}
