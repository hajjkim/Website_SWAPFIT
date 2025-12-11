using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Models;
using SWAPFIT.Data;
using System.Linq;

namespace SWAPFIT.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📦 Hiển thị danh sách thương hiệu
        public IActionResult Index()
        {
            var brands = _context.ThuongHieus.OrderBy(t => t.TenThuongHieu).ToList();
            return View("~/Views/Admin/Brands/Index.cshtml", brands);
        }

        // ➕ Thêm thương hiệu mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBrand(ThuongHieu model)
        {
            if (ModelState.IsValid)
            {
                _context.ThuongHieus.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "✅ Thêm thương hiệu thành công.";
            }
            else
            {
                TempData["Error"] = "❌ Dữ liệu không hợp lệ, vui lòng kiểm tra lại.";
            }

            return RedirectToAction(nameof(Index));
        }

        // 🗑️ Xóa thương hiệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBrand(int id)
        {
            var brand = _context.ThuongHieus.Find(id);
            if (brand != null)
            {
                _context.ThuongHieus.Remove(brand);
                _context.SaveChanges();
                TempData["Success"] = "🗑️ Xóa thương hiệu thành công.";
            }
            else
            {
                TempData["Error"] = "❌ Không tìm thấy thương hiệu cần xóa.";
            }

            return RedirectToAction(nameof(Index));
        }
<<<<<<< HEAD
        // GET: Hiển thị form thêm thương hiệu
        public IActionResult Create()
        {
            return View("~/Views/Brands/Create.cshtml");
        }

        // POST: Xử lý thêm thương hiệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ThuongHieu model)
        {
            if (ModelState.IsValid)
            {
                _context.ThuongHieus.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Thêm thương hiệu thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi → trả lại form với dữ liệu đã nhập
            return View("~/Views/Brands/Create.cshtml", model);
        }
=======
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
    }
}
