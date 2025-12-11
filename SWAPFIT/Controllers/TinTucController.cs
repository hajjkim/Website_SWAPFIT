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

        // ================================
        // 🟢 DANH SÁCH "CHO TẶNG"
        // ================================
<<<<<<< HEAD
        public async Task<IActionResult> ChoTang(
    string? query,                    // TÌM KIẾM
    string? sort,                     // SẮP XẾP
    List<int>? DanhMucIds,
    List<int>? ThuongHieuIds,
    List<string>? Sizes,
    string? Tinh)
        {
            // Bắt đầu query
            var baiViets = _context.BaiViets
=======
        // ================================
        // 🟢 DANH SÁCH "CHO TẶNG"
        // ================================
        public IActionResult ChoTang(List<int>? DanhMucIds, List<int>? ThuongHieuIds, List<string>? Sizes, string? Tinh, string? query)
        {
            var querySearch = _context.BaiViets
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
                .Include(x => x.DanhMuc)
                .Include(x => x.ThuongHieu)
                .Include(x => x.AnhBaiViets)
                .Include(x => x.DiaChi)
<<<<<<< HEAD
                .Where(x => x.LoaiBaiDang == "Tặng" && x.TrangThai == "Đang hiển thị");

            // TÌM KIẾM THEO TÊN HOẶC MÔ TẢ
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim().ToLower();
                baiViets = baiViets.Where(x =>
                    (x.TieuDe != null && x.TieuDe.ToLower().Contains(query)) ||
                    (x.NoiDung != null && x.NoiDung.ToLower().Contains(query)));
            }

            // LỌC DANH MỤC
            if (DanhMucIds != null && DanhMucIds.Any())
                baiViets = baiViets.Where(x => x.MaDanhMuc.HasValue && DanhMucIds.Contains(x.MaDanhMuc.Value));

            // LỌC THƯƠNG HIỆU
            if (ThuongHieuIds != null && ThuongHieuIds.Any())
                baiViets = baiViets.Where(x => x.MaThuongHieu.HasValue && ThuongHieuIds.Contains(x.MaThuongHieu.Value));

            // LỌC SIZE
            if (Sizes != null && Sizes.Any())
                baiViets = baiViets.Where(x => x.Size != null && Sizes.Contains(x.Size));

            // LỌC TỈNH
            if (!string.IsNullOrEmpty(Tinh))
                baiViets = baiViets.Where(x => x.DiaChi != null && x.DiaChi.Tinh == Tinh);

            // SẮP XẾP
            baiViets = sort switch
            {
                "price_asc" => baiViets.OrderBy(x => x.GiaSanPham ?? 0),
                "price_desc" => baiViets.OrderByDescending(x => x.GiaSanPham ?? 0),
                _ => baiViets.OrderByDescending(x => x.NgayTao) // Mới nhất
            };

            // GỬI DỮ LIỆU CHO VIEW (để giữ trạng thái tìm kiếm, lọc)
            ViewBag.CurrentSearch = query;
            ViewBag.CurrentSort = sort;
            ViewBag.DanhMucIds = DanhMucIds ?? new List<int>();
            ViewBag.ThuongHieuIds = ThuongHieuIds;
            ViewBag.Sizes = Sizes ?? new List<string>();
            ViewBag.SelectedTinh = Tinh;

            // Load danh sách lọc
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

            return View(await baiViets.ToListAsync());
        }

        // ================================
        // 🟢 DANH SÁCH "THANH LÝ"
        // ================================
        public async Task<IActionResult> ThanhLy(
    string? query,                    // TÌM KIẾM
    string? sort,                     // SẮP XẾP
    List<int>? DanhMucIds,
    List<int>? ThuongHieuIds,
    List<string>? Sizes,
    string? Tinh)
        {
            // Bắt đầu query: chỉ lấy bài viết "Bán" + đang hiển thị
            var baiViets = _context.BaiViets
=======
                .Where(x => x.LoaiBaiDang == "Tặng"
                         && x.TrangThai != "Ẩn"      // Không lấy bài đã ẩn
                         && x.TrangThai != "Đã xóa"); // Loại bỏ bài đã xóa

            // Kiểm tra nếu có query tìm kiếm theo tên sản phẩm (Tiêu Đề)
            if (!string.IsNullOrEmpty(query))
            {
                querySearch = querySearch.Where(x => x.TieuDe.Contains(query)); // Tìm kiếm theo Tiêu Đề
            }

            // Các bộ lọc khác
            if (DanhMucIds != null && DanhMucIds.Any())
                querySearch = querySearch.Where(x => x.MaDanhMuc.HasValue && DanhMucIds.Contains(x.MaDanhMuc.Value));

            if (ThuongHieuIds != null && ThuongHieuIds.Any())
                querySearch = querySearch.Where(x => x.MaThuongHieu.HasValue && ThuongHieuIds.Contains(x.MaThuongHieu.Value));

            if (Sizes != null && Sizes.Any())
                querySearch = querySearch.Where(x => Sizes.Contains(x.Size));

            if (!string.IsNullOrEmpty(Tinh))
                querySearch = querySearch.Where(x => x.DiaChi != null && x.DiaChi.Tinh == Tinh);

            ViewBag.DanhMucs = _context.DanhMucs.ToList();
            ViewBag.ThuongHieus = _context.ThuongHieus.ToList();

            var tinhs = _context.DiaChis
                .Where(d => d.Tinh != null)
                .Select(d => d.Tinh)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            if (!tinhs.Any())
            {
                tinhs = GetDanhSachTinhMacDinh();
            }

            ViewBag.Tinhs = tinhs;

            return View(querySearch.OrderByDescending(x => x.NgayTao).ToList());
        }


        // ================================
        // 🟢 DANH SÁCH "THANH LÝ"
        // ================================
        public IActionResult ThanhLy(List<int>? DanhMucIds, List<int>? ThuongHieuIds, List<string>? Sizes, string? Tinh, string? query)
        {
            var querySearch = _context.BaiViets
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
                .Include(x => x.DanhMuc)
                .Include(x => x.ThuongHieu)
                .Include(x => x.AnhBaiViets)
                .Include(x => x.DiaChi)
<<<<<<< HEAD
                .Where(x => x.TrangThai == "Đang hiển thị" && x.LoaiBaiDang == "Bán");

            // TÌM KIẾM THEO TIÊU ĐỀ HOẶC MÔ TẢ
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim().ToLower();
                baiViets = baiViets.Where(x =>
                    (x.TieuDe != null && x.TieuDe.ToLower().Contains(query)) ||
                    (x.NoiDung != null && x.NoiDung.ToLower().Contains(query)));
            }

            // LỌC DANH MỤC
            if (DanhMucIds != null && DanhMucIds.Any())
                baiViets = baiViets.Where(x => x.MaDanhMuc.HasValue && DanhMucIds.Contains(x.MaDanhMuc.Value));

            // LỌC THƯƠNG HIỆU
            if (ThuongHieuIds != null && ThuongHieuIds.Any())
                baiViets = baiViets.Where(x => x.MaThuongHieu.HasValue && ThuongHieuIds.Contains(x.MaThuongHieu.Value));

            // LỌC SIZE
            if (Sizes != null && Sizes.Any())
                baiViets = baiViets.Where(x => x.Size != null && Sizes.Contains(x.Size));

            // LỌC TỈNH
            if (!string.IsNullOrEmpty(Tinh))
                baiViets = baiViets.Where(x => x.DiaChi != null && x.DiaChi.Tinh == Tinh);

            // SẮP XẾP
            baiViets = sort switch
            {
                "price_asc" => baiViets.OrderBy(x => x.GiaSanPham ?? 0),
                "price_desc" => baiViets.OrderByDescending(x => x.GiaSanPham ?? 0),
                _ => baiViets.OrderByDescending(x => x.NgayTao) // Mới nhất mặc định
            };

            // GỬI DỮ LIỆU CHO VIEW ĐỂ GIỮ TRẠNG THÁI
            ViewBag.CurrentSearch = query;
            ViewBag.CurrentSort = sort;
            ViewBag.DanhMucIds = DanhMucIds ?? new List<int>();
            ViewBag.ThuongHieuIds = ThuongHieuIds;
            ViewBag.Sizes = Sizes ?? new List<string>();
            ViewBag.SelectedTinh = Tinh;

            // Load danh sách lọc
            ViewBag.DanhMucs = await _context.DanhMucs.ToListAsync();
            ViewBag.ThuongHieus = await _context.ThuongHieus.ToListAsync();

            var tinhs = await _context.DiaChis
                .Where(d => d.Tinh != null)
                .Select(d => d.Tinh!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            if (!tinhs.Any())
                tinhs = GetDanhSachTinhMacDinh(); // Hàm bạn đã có

            ViewBag.Tinhs = tinhs;

            return View(await baiViets.ToListAsync());
        }


=======
                .Where(x => x.LoaiBaiDang == "Bán"
                          && x.TrangThai != "Ẩn"      // Không lấy bài đã ẩn
                          && x.TrangThai != "Đã xóa"); // Loại bỏ bài đã xóa

            // Kiểm tra nếu có query tìm kiếm theo tên sản phẩm (Tiêu Đề)
            if (!string.IsNullOrEmpty(query))
            {
                querySearch = querySearch.Where(x => x.TieuDe.Contains(query)); // Tìm kiếm theo Tiêu Đề
            }

            // Các bộ lọc khác
            if (DanhMucIds != null && DanhMucIds.Any())
                querySearch = querySearch.Where(x => DanhMucIds.Contains(x.MaDanhMuc ?? 0)); // default value 0 or any fallback

            if (ThuongHieuIds != null && ThuongHieuIds.Any())
                querySearch = querySearch.Where(x => x.MaThuongHieu.HasValue && ThuongHieuIds.Contains(x.MaThuongHieu.Value));

            if (Sizes != null && Sizes.Any())
                querySearch = querySearch.Where(x => Sizes.Contains(x.Size));

            if (!string.IsNullOrEmpty(Tinh))
                querySearch = querySearch.Where(x => x.DiaChi != null && x.DiaChi.Tinh == Tinh);

            ViewBag.DanhMucs = _context.DanhMucs.ToList();
            ViewBag.ThuongHieus = _context.ThuongHieus.ToList();

            return View(querySearch.OrderByDescending(x => x.NgayTao).ToList());
        }





>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        // ================================
        // 🟢 CHI TIẾT BÀI VIẾT / SẢN PHẨM
        // ================================
        public IActionResult ChiTiet(int id)
        {
            var baiViet = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Include(b => b.DanhMuc)
                .FirstOrDefault(b => b.MaBaiViet == id);

            if (baiViet == null)
                return NotFound();

            // ⭐ LOAD NGƯỜI BÁN CHẮC CHẮN 100%
            baiViet.NguoiDung = _context.NguoiDungs
                .FirstOrDefault(u => u.MaNguoiDung == baiViet.MaNguoiDung);

            // Load bình luận
            ViewBag.BinhLuans = _context.BinhLuanTinTucs
                .Where(x => x.MaTinTuc == id)
                .Include(x => x.NguoiDung)
                .OrderByDescending(x => x.NgayBinhLuan)
                .ToList();

            // Sản phẩm liên quan - Thêm điều kiện để loại bỏ sản phẩm đã xóa
            ViewBag.SanPhamLienQuan = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Where(b => b.MaDanhMuc == baiViet.MaDanhMuc
                         && b.MaBaiViet != baiViet.MaBaiViet
                         && b.LoaiBaiDang == baiViet.LoaiBaiDang
                         && b.TrangThai != "Đã xóa")  // Điều kiện loại bỏ bài viết đã xóa
                .OrderByDescending(b => b.NgayTao)
                .Take(4)
                .ToList();

            return View(baiViet);
        }


<<<<<<< HEAD
=======



>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        // ================================
        // 🟢 THÊM BÌNH LUẬN
        // ================================
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




        // ================================
        // 🔹 Danh sách tỉnh fallback
        // ================================
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
