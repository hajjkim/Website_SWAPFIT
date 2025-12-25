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

       
        public IActionResult Index()
        {
            var diaChis = _context.DiaChis.ToList();
            return View(diaChis);
        }

        
        public IActionResult Create()
        {
            
            ViewBag.NguoiDungs = _context.NguoiDungs.ToList();
            return View();
        }

       
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
