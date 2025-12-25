using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Models;
using SWAPFIT.Data;
using System.Linq;

namespace SWAPFIT.Controllers
{
    public class DiaChiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiaChiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách địa chỉ
        public IActionResult Index()
        {
            var diaChis = _context.DiaChis.ToList();
            return View(diaChis);
        }

        // GET: /DiaChi/Create
        public IActionResult Create()
        {
            // Nếu cần dropdown chọn người dùng
            ViewBag.NguoiDungs = _context.NguoiDungs.ToList();
            return View();
        }

        // POST: /DiaChi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DiaChi diaChi)
        {
            if (ModelState.IsValid)
            {
                _context.DiaChis.Add(diaChi);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.NguoiDungs = _context.NguoiDungs.ToList();
            return View(diaChi);
        }
    }
}
