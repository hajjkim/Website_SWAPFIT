using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Models;
using SWAPFIT.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWAPFIT.Controllers
{
    public class TinTucController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TinTucController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> ChoTang(
                string? query,                  
                string? sort,                     
                List<int>? DanhMucIds,
                List<int>? ThuongHieuIds,
                List<string>? Sizes,
                string? Tinh,
                 int page = 1,
                int pageSize = 9)
        {
           
            var baiViets = _context.BaiViets
                .Include(x => x.DanhMuc)
                .Include(x => x.ThuongHieu)
                .Include(x => x.AnhBaiViets)
                .Include(x => x.DiaChi)
                .Where(x => x.LoaiBaiDang == "Tặng" && x.TrangThai == "Đang hiển thị");
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim().ToLower();
                baiViets = baiViets.Where(x =>
                    (x.TieuDe != null && x.TieuDe.ToLower().Contains(query)) ||
                    (x.NoiDung != null && x.NoiDung.ToLower().Contains(query)));
            }
            if (DanhMucIds != null && DanhMucIds.Any())
                baiViets = baiViets.Where(x => x.MaDanhMuc.HasValue && DanhMucIds.Contains(x.MaDanhMuc.Value));

            if (ThuongHieuIds != null && ThuongHieuIds.Any())
                baiViets = baiViets.Where(x => x.MaThuongHieu.HasValue && ThuongHieuIds.Contains(x.MaThuongHieu.Value));

            if (Sizes != null && Sizes.Any())
                baiViets = baiViets.Where(x => x.Size != null && Sizes.Contains(x.Size));

            if (!string.IsNullOrEmpty(Tinh))
                baiViets = baiViets.Where(x => x.DiaChi != null && x.DiaChi.Tinh == Tinh);

            baiViets = sort switch
            {
                "price_asc" => baiViets.OrderBy(x => x.GiaSanPham ?? 0),
                "price_desc" => baiViets.OrderByDescending(x => x.GiaSanPham ?? 0),
                _ => baiViets.OrderByDescending(x => x.NgayTao) 
            };
            ViewBag.CurrentSearch = query;
            ViewBag.CurrentSort = sort;
            ViewBag.DanhMucIds = DanhMucIds ?? new List<int>();
            ViewBag.ThuongHieuIds = ThuongHieuIds;
            ViewBag.Sizes = Sizes ?? new List<string>();
            ViewBag.SelectedTinh = Tinh;
            ViewBag.DanhMucs = await _context.DanhMucs.ToListAsync();
            ViewBag.ThuongHieus = await _context.ThuongHieus.ToListAsync();

            var tinhs = await _context.DiaChis
                .Where(d => d.Tinh != null)
                .Select(d => d.Tinh!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            if (!tinhs.Any())
                tinhs = GetDanhSachTinhMacDinh();
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 9;

            var totalItems = await baiViets.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (totalPages > 0 && page > totalPages) page = totalPages;

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            var data = await baiViets
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> ThanhLy(
            string? query,                    
            string? sort,                     
            List<int>? DanhMucIds,
            List<int>? ThuongHieuIds,
            List<string>? Sizes,
            string? Tinh,
             int page = 1,
                int pageSize = 9)
        {            var baiViets = _context.BaiViets
                .Include(x => x.DanhMuc)
                .Include(x => x.ThuongHieu)
                .Include(x => x.AnhBaiViets)
                .Include(x => x.DiaChi)
                .Where(x => x.TrangThai == "Đang hiển thị" && x.LoaiBaiDang == "Bán");
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim().ToLower();
                baiViets = baiViets.Where(x =>
                    (x.TieuDe != null && x.TieuDe.ToLower().Contains(query)) ||
                    (x.NoiDung != null && x.NoiDung.ToLower().Contains(query)));
            }
            if (DanhMucIds != null && DanhMucIds.Any())
                baiViets = baiViets.Where(x => x.MaDanhMuc.HasValue && DanhMucIds.Contains(x.MaDanhMuc.Value));
            if (ThuongHieuIds != null && ThuongHieuIds.Any())
                baiViets = baiViets.Where(x => x.MaThuongHieu.HasValue && ThuongHieuIds.Contains(x.MaThuongHieu.Value));

            if (Sizes != null && Sizes.Any())
                baiViets = baiViets.Where(x => x.Size != null && Sizes.Contains(x.Size));

            if (!string.IsNullOrEmpty(Tinh))
                baiViets = baiViets.Where(x => x.DiaChi != null && x.DiaChi.Tinh == Tinh);

            baiViets = sort switch
            {
                "price_asc" => baiViets.OrderBy(x => x.GiaSanPham ?? 0),
                "price_desc" => baiViets.OrderByDescending(x => x.GiaSanPham ?? 0),
                _ => baiViets.OrderByDescending(x => x.NgayTao) 
            };
            ViewBag.CurrentSearch = query;
            ViewBag.CurrentSort = sort;
            ViewBag.DanhMucIds = DanhMucIds ?? new List<int>();
            ViewBag.ThuongHieuIds = ThuongHieuIds;
            ViewBag.Sizes = Sizes ?? new List<string>();
            ViewBag.SelectedTinh = Tinh;
            ViewBag.DanhMucs = await _context.DanhMucs.ToListAsync();
            ViewBag.ThuongHieus = await _context.ThuongHieus.ToListAsync();

            var tinhs = await _context.DiaChis
                .Where(d => d.Tinh != null)
                .Select(d => d.Tinh!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            if (!tinhs.Any())
                tinhs = GetDanhSachTinhMacDinh(); 

            ViewBag.Tinhs = tinhs;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 9;

            var totalItems = await baiViets.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (totalPages > 0 && page > totalPages) page = totalPages;

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            var data = await baiViets
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return View(data);
           
        }
        public IActionResult ChiTiet(int id)
        {
            var baiViet = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Include(b => b.DanhMuc)
                .FirstOrDefault(b => b.MaBaiViet == id);

            if (baiViet == null)
                return NotFound();

            baiViet.NguoiDung = _context.NguoiDungs
                .FirstOrDefault(u => u.MaNguoiDung == baiViet.MaNguoiDung);

            ViewBag.BinhLuans = _context.BinhLuanTinTucs
                .Where(x => x.MaTinTuc == id)
                .Include(x => x.NguoiDung)
                .OrderByDescending(x => x.NgayBinhLuan)
                .ToList();

            ViewBag.SanPhamLienQuan = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Where(b => b.MaDanhMuc == baiViet.MaDanhMuc
                         && b.MaBaiViet != baiViet.MaBaiViet
                         && b.LoaiBaiDang == baiViet.LoaiBaiDang
                         && b.TrangThai != "Đã xóa")  
                .OrderByDescending(b => b.NgayTao)
                .Take(4)
                .ToList();

            return View(baiViet);
        }
        [HttpPost]
        public async Task<IActionResult> ThemBinhLuan(int baiVietId, string noiDung, int? parentId)
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(noiDung))
                return RedirectToAction("ChiTiet", new { id = baiVietId });

            var bl = new BinhLuanTinTuc
            {
                MaTinTuc = baiVietId,
                MaNguoiDung = userId.Value,
                NoiDung = noiDung.Trim(),
                NgayBinhLuan = DateTime.Now,
                ParentId = parentId   // ⭐ THÊM DÒNG NÀY
            };

            _context.BinhLuanTinTucs.Add(bl);
            await _context.SaveChangesAsync();

            return RedirectToAction("ChiTiet", new { id = baiVietId });
        }

        private List<string> GetDanhSachTinhMacDinh()
        {
            return new List<string>
            {
                "An Giang","Bà Rịa - Vũng Tàu","Bắc Giang","Bắc Kạn","Bạc Liêu","Bắc Ninh","Bến Tre","Bình Dương",
                "Bình Định","Bình Phước","Bình Thuận","Cà Mau","Cao Bằng","Đắk Lắk","Đắk Nông","Điện Biên",
                "Đồng Nai","Đồng Tháp","Gia Lai","Hà Giang","Hà Nam","Hà Nội","Hà Tĩnh","Hải Dương","Hải Phòng",
                "Hậu Giang","Hòa Bình","Hưng Yên","Khánh Hòa","Kiên Giang","Kon Tum","Lai Châu","Lâm Đồng",
                "Lạng Sơn","Lào Cai","Long An","Nam Định","Nghệ An","Ninh Bình","Ninh Thuận","Phú Thọ","Phú Yên",
                "Quảng Bình","Quảng Nam","Quảng Ngãi","Quảng Ninh","Quảng Trị","Sóc Trăng","Sơn La","Tây Ninh",
                "Thái Bình","Thái Nguyên","Thanh Hóa","Thừa Thiên Huế","Tiền Giang","TP Hồ Chí Minh","Trà Vinh",
                "Tuyên Quang","Vĩnh Long","Vĩnh Phúc","Yên Bái","Đà Nẵng","Cần Thơ"
            };
        }
    }
}
