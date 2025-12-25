using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Models;
using SWAPFIT.Data;
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

       
        public IActionResult Index()
        {
           
            var danhMucs = _context.DanhMucs.ToList();

            return View(danhMucs);
        }

       
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
