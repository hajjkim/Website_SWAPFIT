using SWAPFIT.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SWAPFIT.Controllers
{
    public class BaiVietController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BaiVietController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ==========================================
        // HIỂN THỊ FORM ĐĂNG BÀI
        // ==========================================
        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null)
            {
                TempData["Error"] = "⚠️ Bạn cần đăng nhập trước khi đăng bài.";
                return RedirectToAction("Login", "Account");
            }

            LoadDropdownData();
            return View();
        }

        // ==========================================
        // XỬ LÝ GỬI FORM ĐĂNG BÀI
        // ==========================================
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(BaiViet baiViet, List<IFormFile> images)
        //{
        //    // ===== KIỂM TRA DỮ LIỆU =====
        //    if (string.IsNullOrWhiteSpace(baiViet.TieuDe) ||
        //        baiViet.MaDanhMuc == null ||
        //        baiViet.MaThuongHieu == null)
        //    {
        //        ModelState.AddModelError("", "⚠️ Vui lòng nhập đầy đủ thông tin bắt buộc.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        LoadDropdownData();
        //        return View(baiViet);
        //    }

        //    baiViet.NgayTao = DateTime.Now;
        //    baiViet.TrangThai = "Chờ duyệt";
        //    baiViet.MaNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung") ?? 0;

        //    if (baiViet.LoaiBaiDang == "Tặng")
        //        baiViet.GiaSanPham = 0;

        //    _context.Add(baiViet);
        //    await _context.SaveChangesAsync();

        //    // ===== LƯU ẢNH =====
        //    if (images != null && images.Count > 0)
        //    {
        //        foreach (var img in images)
        //        {
        //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
        //            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "posts");

        //            if (!Directory.Exists(uploadDir))
        //                Directory.CreateDirectory(uploadDir);

        //            var filePath = Path.Combine(uploadDir, fileName);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await img.CopyToAsync(stream);
        //            }

        //            var anh = new AnhBaiViet
        //            {
        //                MaBaiViet = baiViet.MaBaiViet,
        //                DuongDan = "/images/posts/" + fileName
        //            };
        //            _context.AnhBaiViets.Add(anh);
        //        }
        //        await _context.SaveChangesAsync();
        //    }

        //    TempData["Message"] = "🎉 Bài viết của bạn đã được gửi và đang chờ xét duyệt.";
        //    return RedirectToAction("ThongBaoChoDuyet");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(BaiViet model, List<IFormFile> AnhSanPham)
        //{
        //    // Lấy id người dùng đăng bài
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (maNguoiDung == null)
        //        return RedirectToAction("Login", "Account");

        //    if (!ModelState.IsValid)
        //    {
        //        // load lại dropdown nếu có
        //        ViewBag.DanhMucs = _context.DanhMucs.ToList();
        //        ViewBag.ThuongHieus = _context.ThuongHieus.ToList();
        //        return View(model);
        //    }

        //    model.MaNguoiDung = maNguoiDung.Value;

        //    model.NgayTao = DateTime.Now;
        //    model.TrangThai = "Chờ duyệt";

        //    //_context.BaiViets.Add(model);
        //    //await _context.SaveChangesAsync();   // để có MaBaiViet

        //    //// THƯ MỤC LƯU ẢNH: wwwroot/images/posts
        //    //var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "posts");
        //    //if (!Directory.Exists(uploadFolder))
        //    //    Directory.CreateDirectory(uploadFolder);

        //    //if (AnhSanPham != null && AnhSanPham.Count > 0)
        //    //{
        //    //    foreach (var file in AnhSanPham)
        //    //    {
        //    //        if (file != null && file.Length > 0)
        //    //        {
        //    //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //    //            var filePath = Path.Combine(uploadFolder, fileName);

        //    //            using (var stream = new FileStream(filePath, FileMode.Create))
        //    //            {
        //    //                await file.CopyToAsync(stream);
        //    //            }

        //    //            var anh = new AnhBaiViet
        //    //            {
        //    //                MaBaiViet = model.MaBaiViet,
        //    //                DuongDan = "/images/posts/" + fileName
        //    //            };

        //    //            _context.AnhBaiViets.Add(anh);
        //    //        }
        //    //    }

        //    //    await _context.SaveChangesAsync();
        //    //}

        //    //TempData["Success"] = "Đăng bài thành công, vui lòng chờ admin duyệt!";
        //    //return RedirectToAction("ThongBaoChoDuyet");
        //    try
        //    {
        //        // Tạm thời vô hiệu hóa trigger trước khi thực hiện thao tác lưu
        //        await _context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER trg_OnlyUpdateNguoiDungMaNguoiDung ON BaiViets");

        //        // Thêm bài viết vào cơ sở dữ liệu
        //        _context.BaiViets.Add(model);
        //        await _context.SaveChangesAsync();   // Lưu bài viết để có MaBaiViet

        //        // THƯ MỤC LƯU ẢNH: wwwroot/images/posts
        //        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "posts");
        //        if (!Directory.Exists(uploadFolder))
        //            Directory.CreateDirectory(uploadFolder);

        //        if (AnhSanPham != null && AnhSanPham.Count > 0)
        //        {
        //            foreach (var file in AnhSanPham)
        //            {
        //                if (file != null && file.Length > 0)
        //                {
        //                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //                    var filePath = Path.Combine(uploadFolder, fileName);

        //                    using (var stream = new FileStream(filePath, FileMode.Create))
        //                    {
        //                        await file.CopyToAsync(stream);
        //                    }

        //                    var anh = new AnhBaiViet
        //                    {
        //                        MaBaiViet = model.MaBaiViet,
        //                        DuongDan = "/images/posts/" + fileName
        //                    };

        //                    _context.AnhBaiViets.Add(anh);
        //                }
        //            }

        //            await _context.SaveChangesAsync();
        //        }

        //        // Kích hoạt lại trigger sau khi hoàn tất cập nhật
        //        await _context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER trg_OnlyUpdateNguoiDungMaNguoiDung ON BaiViets");

        //        TempData["Success"] = "Đăng bài thành công, vui lòng chờ admin duyệt!";
        //        return RedirectToAction("ThongBaoChoDuyet");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ghi log hoặc xử lý lỗi ở đây
        //        TempData["Error"] = "Đã xảy ra lỗi trong quá trình tạo bài viết.";
        //        Console.WriteLine(ex.Message);
        //        return View(model);
        //    }
        //}



      
        public async Task<IActionResult> Create(BaiViet model, List<IFormFile> AnhSanPham)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                LoadDropdownData();
                return View(model);
            }

            // CHỈ CẦN GÁN MaNguoiDung là đủ – EF sẽ tự hiểu!
            model.MaNguoiDung = maNguoiDung.Value;
            model.NgayTao = DateTime.Now;
            model.TrangThai = "Chờ duyệt";

            if (model.LoaiBaiDang == "Tặng")
                model.GiaSanPham = 0;

            try
            {
                // Nếu bạn KHÔNG có trigger chặn update MaNguoiDung thì KHÔNG CẦN disable trigger
                // Nếu có trigger thì để lại 2 dòng dưới, còn không thì xóa luôn cho nhẹ
                // await _context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER ALL ON BaiViets");

                _context.BaiViets.Add(model);
                await _context.SaveChangesAsync(); // lúc này model.MaBaiViet đã có giá trị

                // === LƯU ẢNH (bắt buộc phải có) ===
                if (AnhSanPham != null && AnhSanPham.Count > 0)
                {
                    var uploadFolder = Path.Combine(_env.WebRootPath, "images", "posts");
                    Directory.CreateDirectory(uploadFolder);

                    foreach (var file in AnhSanPham)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadFolder, fileName);

                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);

                            _context.AnhBaiViets.Add(new AnhBaiViet
                            {
                                MaBaiViet = model.MaBaiViet,
                                DuongDan = "/images/posts/" + fileName
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Đăng bài thành công! Đang chờ duyệt...";
                return RedirectToAction("ThongBaoChoDuyet");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi khi đăng bài: " + ex.Message;
                LoadDropdownData();
                return View(model);
            }
        }



        public IActionResult ThongBaoChoDuyet()
        {
            return View();
        }


        // ==========================================
        // API LẤY SIZE THEO DANH MỤC
        // ==========================================
        [HttpGet]
        public JsonResult GetSizesByCategory(int categoryId)
        {
            var category = _context.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == categoryId);

            if (category == null)
                return Json(new List<string>());

            string name = category.TenDanhMuc.ToLower();
            List<string> sizes = new List<string>();

            if (name.Contains("áo") || name.Contains("quần") || name.Contains("váy"))
            {
                sizes = new List<string> { "S", "M", "L", "XL", "XXL" };
            }
            else if (name.Contains("giày") || name.Contains("dép"))
            {
                sizes = Enumerable.Range(35, 10).Select(i => i.ToString()).ToList(); // 35 - 44
            }

            return Json(sizes);
        }


        // ==========================================
        // CHO TẶNG
        // ==========================================
    //    public IActionResult ChoTang(
    //int[] DanhMucIds = null,
    //int[] ThuongHieuIds = null,
    //string[] Sizes = null)
    //    {
    //        ViewBag.DanhMucs = _context.DanhMucs.ToList();
    //        ViewBag.ThuongHieus = _context.ThuongHieus.ToList();

    //        // Luôn KHỞI TẠO ViewBag.Sizes để tránh null
    //        ViewBag.Sizes = new List<string>
    //{
    //    "S","M","L","XL","XXL","35","36","37","38","39","40","41","42","43","44"
    //};

    //        var query = _context.BaiViets
    //            .Include(b => b.AnhBaiViets)
    //            .Where(b => b.TrangThai == "Đang hiển thị" && b.LoaiBaiDang == "Tặng");

    //        if (DanhMucIds?.Any() == true)
    //            query = query.Where(b => DanhMucIds.Contains(b.MaDanhMuc ?? 0));

    //        if (ThuongHieuIds?.Any() == true)
    //            query = query.Where(b => ThuongHieuIds.Contains(b.MaThuongHieu ?? 0));

    //        if (Sizes?.Any() == true)
    //            query = query.Where(b => Sizes.Contains(b.Size));

    //        return View(query.OrderByDescending(b => b.NgayTao).ToList());
    //    }



    //    // ==========================================
    //    // THANH LÝ
    //    // ==========================================
    //    public IActionResult ThanhLy()
    //    {
    //        ViewBag.DanhMucs = _context.DanhMucs.ToList();
    //        ViewBag.ThuongHieus = _context.ThuongHieus.ToList();

    //        // THÊM DÒNG NÀY — bắt buộc!
    //        ViewBag.Sizes = new List<string> { "S", "M", "L", "XL", "XXL", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44" };

    //        var baiViets = _context.BaiViets
    //            .Include(b => b.AnhBaiViets)
    //            .Include(b => b.DanhMuc)
    //            .Include(b => b.ThuongHieu)
    //            .Include(b => b.NguoiDung)
    //            .Where(b => b.TrangThai == "Đang hiển thị" && b.LoaiBaiDang == "Bán")
    //            .OrderByDescending(b => b.NgayTao)
    //            .ToList();

    //        return View(baiViets);
    //    }



        // ==========================================
        // LOAD DROPDOWN DATA
        // ==========================================
        private void LoadDropdownData()
        {
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
        }
    }
}
