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

 
        public IActionResult Index()
        {
            var brands = _context.ThuongHieus.OrderBy(t => t.TenThuongHieu).ToList();
            return View("~/Views/Admin/Brands/Index.cshtml", brands);
        }

      
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
        
        public IActionResult Create()
        {
            return View("~/Views/Brands/Create.cshtml");
        }

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

            return View("~/Views/Brands/Create.cshtml", model);
        }
    }
}
