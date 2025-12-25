using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Data;
using SWAPFIT.Models;
using SWAPFIT.Model;   // nếu UserVoucher nằm trong namespace này
using System;
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

            ViewBag.ActiveVouchers = activeVouchers;

            return View(danhMucs);
        }


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
        }
    }
}
