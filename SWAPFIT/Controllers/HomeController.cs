using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Data;
using SWAPFIT.Models;
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
            ViewBag.ActiveVouchers = activeVouchers;

            return View(danhMucs);
        }


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
