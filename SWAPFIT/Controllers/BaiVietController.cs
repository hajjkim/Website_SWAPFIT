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
<<<<<<< HEAD
        public IActionResult Create()
        {
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");

            var baiViet = new BaiViet
            {
                AnhBaiViets = new List<AnhBaiViet>()  // ← Thêm dòng này là hết lỗi ngay!
            };

            return View(baiViet);
=======
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
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
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
<<<<<<< HEAD

=======
        [HttpPost]
        [ValidateAntiForgeryToken]
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
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



<<<<<<< HEAD
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(BaiViet model, List<IFormFile> AnhSanPham)
        //{

        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (maNguoiDung == null)
        //        return RedirectToAction("Login", "Account");

        //    if (!ModelState.IsValid)
        //    {
        //        // Load lại dữ liệu dropdown nếu có lỗi
        //        LoadDropdownData();
        //        return View(model);
        //    }

        //    model.MaNguoiDung = maNguoiDung.Value;
        //    model.NgayTao = DateTime.Now;
        //    model.TrangThai = "Chờ duyệt";

        //    // Xử lý các ảnh được upload
        //    if (AnhSanPham != null && AnhSanPham.Count > 0)
        //    {
        //        var uploadFolder = Path.Combine(_env.WebRootPath, "images", "posts");
        //        Directory.CreateDirectory(uploadFolder);

        //        foreach (var file in AnhSanPham)
        //        {
        //            if (file.Length > 0)
        //            {
        //                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //                var filePath = Path.Combine(uploadFolder, fileName);

        //                using var stream = new FileStream(filePath, FileMode.Create);
        //                await file.CopyToAsync(stream);

        //                _context.AnhBaiViets.Add(new AnhBaiViet
        //                {
        //                    MaBaiViet = model.MaBaiViet,
        //                    DuongDan = "/images/posts/" + fileName // Lưu đường dẫn ảnh đúng
        //                });
        //            }
        //        }

        //        await _context.SaveChangesAsync(); // Lưu ảnh vào CSDL
        //    }


        //    // Lưu bài viết vào cơ sở dữ liệu
        //    _context.BaiViets.Add(model);
        //    await _context.SaveChangesAsync();

        //    TempData["Success"] = "Đăng bài thành công! Đang chờ duyệt...";
        //    return RedirectToAction("ThongBaoChoDuyet");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BaiViet baiViet, List<IFormFile> AnhSanPham)
        {
            // KIỂM TRA ĐĂNG NHẬP
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
            {
                TempData["Error"] = "Vui lòng đăng nhập để đăng bài!";
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // GÁN NGƯỜI DÙNG ĐĂNG BÀI
                baiViet.MaNguoiDung = maNguoiDung.Value;
                baiViet.NgayTao = DateTime.Now;
                baiViet.TrangThai = "Chờ duyệt"; // hoặc "Đang hiển thị" tùy bạn

                // Nếu là tặng thì giá = 0
                if (baiViet.LoaiBaiDang == "Tặng")
                    baiViet.GiaSanPham = 0;

                // Bước 1: Lưu bài viết trước để lấy MaBaiViet
                _context.BaiViets.Add(baiViet);
                await _context.SaveChangesAsync(); // Bây giờ MaBaiViet đã có giá trị

                // Bước 2: Lưu ảnh (nếu có)
                if (AnhSanPham != null && AnhSanPham.Count > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "AnhBaiViet");
                    Directory.CreateDirectory(uploadFolder); // tạo thư mục nếu chưa có
=======
      
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
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff

                    foreach (var file in AnhSanPham)
                    {
                        if (file.Length > 0)
                        {
<<<<<<< HEAD
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var anh = new AnhBaiViet
                            {
                                MaBaiViet = baiViet.MaBaiViet,
                                DuongDan = "/Upload/AnhBaiViet/" + fileName
                            };
                            _context.AnhBaiViets.Add(anh);
=======
                            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadFolder, fileName);

                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);

                            _context.AnhBaiViets.Add(new AnhBaiViet
                            {
                                MaBaiViet = model.MaBaiViet,
                                DuongDan = "/images/posts/" + fileName
                            });
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Đăng bài thành công! Đang chờ duyệt...";
                return RedirectToAction("ThongBaoChoDuyet");
            }
<<<<<<< HEAD

            // Nếu có lỗi validate → load lại dropdown
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
            return View(baiViet);
        }

        //public async Task<IActionResult> Create(BaiViet baiViet, List<IFormFile> AnhSanPham)
        //{
        //    // KIỂM TRA ĐĂNG NHẬP
        //    var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (!maNguoiDung.HasValue)
        //    {
        //        TempData["Error"] = "Vui lòng đăng nhập để đăng bài!";
        //        return RedirectToAction("Login", "Account");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // GÁN NGƯỜI DÙNG ĐĂNG BÀI
        //        baiViet.MaNguoiDung = maNguoiDung.Value;
        //        baiViet.NgayTao = DateTime.Now;
        //        baiViet.TrangThai = "Chờ duyệt"; // hoặc "Đang hiển thị" tùy bạn

        //        // Nếu là tặng thì giá = 0
        //        if (baiViet.LoaiBaiDang == "Tặng")
        //            baiViet.GiaSanPham = 0;

        //        // Debug: Kiểm tra dữ liệu của bài viết trước khi insert
        //        Console.WriteLine($"Creating BaiViet: TieuDe={baiViet.TieuDe}, MaNguoiDung={baiViet.MaNguoiDung}, LoaiBaiDang={baiViet.LoaiBaiDang}");

        //        // Bước 1: Insert the BaiViet record using SQL Raw, không cần thêm MaBaiViet vì nó tự động tăng
        //        string insertBaiVietQuery = @"
        //    INSERT INTO BaiViets (TieuDe, MaNguoiDung, LoaiBaiDang, GiaSanPham, TrangThai, NgayTao, SoLuong, NoiDung, MaDanhMuc, MaThuongHieu, LyDo)
        //    VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10});
        //    SELECT CAST(SCOPE_IDENTITY() AS INT);"; // To get the inserted MaBaiViet

        //        try
        //        {
        //            // Debug: Log the SQL query before executing
        //            Console.WriteLine("Executing Insert SQL: " + insertBaiVietQuery);

        //            // Execute the SQL query and get the MaBaiViet
        //            var maBaiViet = await _context.BaiViets
        //                .FromSqlRaw(insertBaiVietQuery,
        //                    baiViet.TieuDe,
        //                    baiViet.MaNguoiDung,
        //                    baiViet.LoaiBaiDang,
        //                    baiViet.GiaSanPham,
        //                    baiViet.TrangThai,
        //                    baiViet.NgayTao,
        //                    baiViet.SoLuong,
        //                    baiViet.NoiDung,
        //                    baiViet.MaDanhMuc,
        //                    baiViet.MaThuongHieu,
        //                    baiViet.LyDo)
        //                .Select(b => b.MaBaiViet)
        //                .FirstOrDefaultAsync();

        //            // Debug: Log the MaBaiViet to check if it's returned correctly
        //            Console.WriteLine($"Post inserted. MaBaiViet: {maBaiViet}");

        //            if (maBaiViet == 0)
        //            {
        //                TempData["Error"] = "Có lỗi khi tạo bài viết.";
        //                return RedirectToAction("Create");
        //            }

        //            // Step 2: Process images if any
        //            if (AnhSanPham != null && AnhSanPham.Any())
        //            {
        //                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "AnhBaiViet");
        //                Directory.CreateDirectory(uploadFolder); // Create the folder if not exists

        //                foreach (var file in AnhSanPham)
        //                {
        //                    if (file.Length > 0)
        //                    {
        //                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //                        var filePath = Path.Combine(uploadFolder, fileName);

        //                        // Debug: Log file information
        //                        Console.WriteLine($"Uploading image: {fileName} to path: {filePath}");

        //                        using (var stream = new FileStream(filePath, FileMode.Create))
        //                        {
        //                            await file.CopyToAsync(stream);
        //                        }

        //                        // Insert image using raw SQL
        //                        string insertImageQuery = "INSERT INTO AnhBaiViets (MaBaiViet, DuongDan) VALUES ({0}, {1})";
        //                        // Debug: Log the insert image query
        //                        Console.WriteLine("Inserting image SQL: " + insertImageQuery);

        //                        await _context.Database.ExecuteSqlRawAsync(insertImageQuery, maBaiViet, "/Upload/AnhBaiViet/" + fileName);
        //                    }
        //                }
        //            }

        //            TempData["Success"] = "Đăng bài thành công! Đang chờ duyệt...";
        //            return RedirectToAction("ThongBaoChoDuyet");
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log the error
        //            Console.WriteLine("Error during insert: " + ex.Message);
        //            TempData["Error"] = "Có lỗi khi đăng bài. Vui lòng thử lại!";
        //            return RedirectToAction("Create");
        //        }
        //    }

        //    // If validation fails, reload the dropdown lists
        //    ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
        //    ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
        //    return View(baiViet);
        //}



=======
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi khi đăng bài: " + ex.Message;
                LoadDropdownData();
                return View(model);
            }
        }

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff


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
<<<<<<< HEAD
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
=======
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
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff



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
