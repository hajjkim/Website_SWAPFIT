using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Models;
using SWAPFIT.Data;// hoặc namespace chứa ApplicationDbContext
using System.Linq;

namespace SWAPFIT.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị danh sách danh mục
        public IActionResult Index()
        {
            // Lấy toàn bộ danh mục từ DB
            var danhMucs = _context.DanhMucs.ToList();

            // Truyền danh sách này sang View
            return View(danhMucs);
        }

        // ✅ Tạo danh mục mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DanhMuc model)
        {
            if (ModelState.IsValid)
            {
                _context.DanhMucs.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        // ✅ Sửa danh mục
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var danhMuc = _context.DanhMucs.Find(id);
            if (danhMuc == null) return NotFound();
            return View(danhMuc);
        }

        [HttpPost]
        public IActionResult Edit(DanhMuc model)
        {
            if (ModelState.IsValid)
            {
                _context.DanhMucs.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // ✅ Xóa danh mục
        public IActionResult Delete(int id)
        {
            var danhMuc = _context.DanhMucs.Find(id);
            if (danhMuc == null) return NotFound();

            _context.DanhMucs.Remove(danhMuc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
