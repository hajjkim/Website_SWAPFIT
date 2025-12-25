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
        public IActionResult Index()
        {
            var danhMucs = _context.DanhMucs.ToList();
            var now = DateTime.Now;

            var activeVouchers = _context.UuDais
                .Where(v =>
                    v.TrangThai.Trim() == "HoatDong" &&
                    v.NgayBatDau <= now &&
                    v.NgayKetThuc >= now
                )
                .ToList();

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
