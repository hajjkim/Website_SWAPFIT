using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SWAPFIT.Models;
using SWAPFIT.Data;
using System.IO;
using System.Linq;

namespace SWAPFIT.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================
        // 🔐 KIỂM TRA QUYỀN ADMIN
        // ====================================================
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role != null && role.ToLower() == "admin";
        }

        // ====================================================
        // 🏠 DASHBOARD
        // ====================================================
        public IActionResult Dashboard()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            // --- Thống kê số lượng ---
            ViewBag.TotalUsers = _context.NguoiDungs.Count();
            ViewBag.TotalPosts = _context.BaiViets.Count();
            ViewBag.TotalCategories = _context.DanhMucs.Count();
            ViewBag.TotalBrands = _context.ThuongHieus.Count();
            ViewBag.TotalVouchers = _context.UuDais.Count();

            // 🔹 TOP NGƯỜI DÙNG ĐĂNG NHIỀU BÀI NHẤT (an toàn, không dính null)
            var topNguoiDung = _context.NguoiDungs
                .Select(u => new
                {
                    MaNguoiDung = u.MaNguoiDung,
                    TenNguoiDung = u.HoTen ?? u.TenDangNhap,
                    SoBaiViet = _context.BaiViets.Count(b =>
                        b.MaNguoiDung == u.MaNguoiDung &&
                        b.TrangThai != "Đã xóa")
                })
                .Where(x => x.SoBaiViet > 0)
                .OrderByDescending(x => x.SoBaiViet)
                .Take(5)
                .ToList();

            ViewBag.TopNguoiDungBaiViet = topNguoiDung;

            // --- Bài viết chờ duyệt ---
            // --- Bài viết chờ duyệt (phiên bản hoàn hảo) ---
            var pendingPosts = _context.BaiViets
                .Where(b => b.TrangThai == "Chờ duyệt")
                .OrderByDescending(b => b.NgayTao)
                .Select(b => new
                {
                    b.MaBaiViet,
                    b.TieuDe,
                    b.NgayTao,
                    b.TrangThai,
                    TenNguoiDang = _context.NguoiDungs
                        .Where(u => u.MaNguoiDung == b.MaNguoiDung)
                        .Select(u =>
                            !string.IsNullOrWhiteSpace(u.HoTen) ? u.HoTen :
                            !string.IsNullOrWhiteSpace(u.TenDangNhap) ? u.TenDangNhap :
                            "Người dùng đã xóa"
                        )
                        .FirstOrDefault() ?? "Không xác định"
                })
                .AsEnumerable()
                .Select(x => new BaiViet // ép về BaiViet để View vẫn dùng được
                {
                    MaBaiViet = x.MaBaiViet,
                    TieuDe = x.TieuDe,
                    NgayTao = x.NgayTao,
                    TrangThai = x.TrangThai,
                    NguoiDung = new NguoiDung { HoTen = x.TenNguoiDang }
                })
                .ToList();
            var baoCaoTaiKhoans = _context.BaoCaoTaiKhoans
       .Include(b => b.NguoiBiBaoCao)
       .Include(b => b.NguoiBaoCao)
       .OrderByDescending(b => b.NgayTao)
       .Take(10) // lấy 10 báo cáo mới nhất
       .ToList();

            ViewBag.BaoCaoTaiKhoan = baoCaoTaiKhoans;
            return View(pendingPosts);
        }



        // ====================================================
        // 📝 DANH SÁCH TẤT CẢ BÀI VIẾT
        // ====================================================
        public IActionResult Posts()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            //var posts = _context.BaiViets
            //    .Include(p => p.NguoiDung)  // Đảm bảo bao gồm NguoiDung
            //    .OrderByDescending(p => p.NgayTao)
            //    .AsEnumerable() // Chuyển đổi sang LINQ to Objects (Client-side)
            //    .Select(p => new BaiViet
            //    {
            //        MaBaiViet = p.MaBaiViet,
            //        TieuDe = p.TieuDe,
            //        LoaiBaiDang = p.LoaiBaiDang,
            //        NgayTao = p.NgayTao,
            //        TrangThai = p.TrangThai,
            //        GiaSanPham = p.GiaSanPham,
            //        SoLuong = p.SoLuong,
            //        NoiDung = p.NoiDung,
            //        NguoiDung = p.NguoiDung != null
            //            ? new NguoiDung
            //            {
            //                MaNguoiDung = p.NguoiDung.MaNguoiDung,
            //                HoTen = p.NguoiDung.HoTen ?? p.NguoiDung.TenDangNhap ?? "Tài khoản đã xóa"
            //            }
            //            : new NguoiDung { HoTen = "Người dùng không xác định" }  // Nếu không có người đăng
            //    })
            //    .ToList();
            //foreach (var post in posts)
            //{
            //    Console.WriteLine($"Post ID: {post.MaBaiViet}, Title: {post.TieuDe}, Người đăng: {post.NguoiDung.HoTen}, Status: {post.TrangThai}");
            //}

            var posts = _context.BaiViets
     .Include(p => p.NguoiDung)  // Đảm bảo bao gồm NguoiDung
     .OrderByDescending(p => p.NgayTao)
     .AsEnumerable() // Chuyển đổi sang LINQ to Objects (Client-side)
     .Select(p => new BaiViet
     {
         MaBaiViet = p.MaBaiViet,
         TieuDe = p.TieuDe,
         LoaiBaiDang = p.LoaiBaiDang,
         NgayTao = p.NgayTao,
         TrangThai = p.TrangThai,
         GiaSanPham = p.GiaSanPham,
         SoLuong = p.SoLuong,
         NoiDung = p.NoiDung,
         NguoiDung = p.NguoiDung != null ? new NguoiDung
         {
             MaNguoiDung = p.NguoiDung.MaNguoiDung,
             HoTen = !string.IsNullOrWhiteSpace(p.NguoiDung.HoTen)
                     ? p.NguoiDung.HoTen
                     : !string.IsNullOrWhiteSpace(p.NguoiDung.TenDangNhap)
                         ? p.NguoiDung.TenDangNhap
                         : "Người dùng không xác định"
         }
         : new NguoiDung { HoTen = "Người dùng không xác định" }
     })
     .ToList();

            foreach (var post in posts)
            {
                Console.WriteLine($"Post ID: {post.MaBaiViet}, Title: {post.TieuDe}, Người đăng: {post.NguoiDung?.HoTen ?? "Không có thông tin"}");
                // Kiểm tra thêm chi tiết trong post để đảm bảo rằng 'NguoiDung' được liên kết chính xác
                Console.WriteLine($"NguoiDung - MaNguoiDung: {post.NguoiDung?.MaNguoiDung}, HoTen: {post.NguoiDung?.HoTen}, TenDangNhap: {post.NguoiDung?.TenDangNhap}");
            }


            return View(posts);
        }



        public IActionResult PostDetails(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var post = _context.BaiViets
                .Include(x => x.NguoiDung)
                .Include(x => x.AnhBaiViets)
                .Include(x => x.DanhMuc)
                .FirstOrDefault(x => x.MaBaiViet == id);

            if (post == null)
                return NotFound();
            Console.WriteLine($"Post ID: {post.MaBaiViet}, Title: {post.TieuDe}, Người đăng: {post.NguoiDung?.HoTen ?? "Không có người đăng"}");

            return View(post);   // 👉 mặc định sẽ tìm Views/Admin/PostDetails.cshtml
        }


        // ====================================================
        // 🟢 DUYỆT BÀI
        // ====================================================
        public IActionResult Duyet(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var post = _context.BaiViets.FirstOrDefault(x => x.MaBaiViet == id);
            if (post == null) return NotFound();

            post.TrangThai = "Đang hiển thị";

            // 🔔 Thêm thông báo cho chủ bài viết
            var tb = new ThongBao
            {
                MaNguoiDung = post.MaNguoiDung,                   // người đăng bài
                NoiDung = $"Bài viết \"{post.TieuDe}\" đã được admin duyệt và đang hiển thị.",
                LienKet = Url.Action("Details", "BaiViet",        // tuỳ bạn có action nào
                                     new { id = post.MaBaiViet },
                                     Request.Scheme),
                DaXem = false,
                NgayTao = DateTime.Now
            };
            _context.ThongBaos.Add(tb);

            _context.SaveChanges();

            TempData["Success"] = "Bài viết đã được duyệt!";
            return RedirectToAction(nameof(Dashboard));
        }


        // ====================================================
        // ❌ TỪ CHỐI BÀI – HIỂN THỊ FORM NHẬP LÝ DO
        // ====================================================
        [HttpGet]
        public IActionResult TuChoi(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var post = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == id);
            if (post == null) return NotFound();

            var vm = new TuChoiBaiVietViewModel
            {
                MaBaiViet = post.MaBaiViet,
                TieuDe = post.TieuDe
            };

            return View(vm);   // Views/Admin/TuChoi.cshtml
        }
        // ====================================================
        // ❌ TỪ CHỐI BÀI – LƯU LÝ DO
        // ====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TuChoi(int id, string lyDoTuChoi)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var post = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == id);
            if (post == null) return NotFound();

            post.TrangThai = "Từ chối";
            post.LyDoTuChoi = lyDoTuChoi;   // nếu bạn có field này

            // 🔔 Thông báo cho người đăng
            var tb = new ThongBao
            {
                MaNguoiDung = post.MaNguoiDung,
                NoiDung = $"Bài viết \"{post.TieuDe}\" đã bị từ chối. Lý do: {lyDoTuChoi}",
                LienKet = Url.Action("Details", "BaiViet",
                                     new { id = post.MaBaiViet },
                                     Request.Scheme),
                DaXem = false,
                NgayTao = DateTime.Now
            };
            _context.ThongBaos.Add(tb);

            _context.SaveChanges();

            TempData["Error"] = "Bài viết đã bị từ chối!";
            return RedirectToAction(nameof(Dashboard));
        }



        // ====================================================
        // 🗑️ XÓA BÀI (SOFT DELETE)
        // ====================================================
        [HttpPost]
        // 🗑️ XÓA BÀI (SOFT DELETE)
        [HttpPost]
        public IActionResult DeletePostAdmin(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var post = _context.BaiViets
                .Include(p => p.AnhBaiViets)
                .FirstOrDefault(p => p.MaBaiViet == id);

            if (post == null) return NotFound();

            // Thay đổi trạng thái thành "Đã xóa"
            post.TrangThai = "Đã xóa";
            _context.SaveChanges();

            TempData["Success"] = "Bài viết đã được chuyển sang trạng thái Đã xóa.";
            return RedirectToAction(nameof(Posts));  // Quay lại danh sách bài viết
        }


        // ====================================================
        // 👤 QUẢN LÝ NGƯỜI DÙNG
        // ====================================================
        public IActionResult Users()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var users = _context.NguoiDungs
                .OrderBy(u => u.VaiTro)
                .ToList();

            // 🔹 Top 3 người đăng bài nhiều nhất
            var top3NguoiDung = _context.NguoiDungs
                .Select(u => new
                {
                    MaNguoiDung = u.MaNguoiDung,
                    TenDangNhap = u.TenDangNhap,
                    HoTen = u.HoTen,
                    SoBaiViet = _context.BaiViets.Count(b =>
                        b.MaNguoiDung == u.MaNguoiDung && b.TrangThai != "Đã xóa")
                })
                .Where(x => x.SoBaiViet > 0)                 // chỉ lấy user có bài
                .OrderByDescending(x => x.SoBaiViet)
                .Take(3)
                .ToList();

            ViewBag.Top3NguoiDung = top3NguoiDung;

            return View(users);
        }


        [HttpPost]
        public IActionResult ToggleBlockUser(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var user = _context.NguoiDungs.Find(id);
            if (user == null) return NotFound();

            user.TrangThai = (user.TrangThai == "Hoạt động") ? "Bị khóa" : "Hoạt động";
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật trạng thái người dùng thành công!";
            return RedirectToAction(nameof(Users));
        }

        // ====================================================
        // 🏷️ QUẢN LÝ THƯƠNG HIỆU
        // ====================================================
        public IActionResult Brands()
        {
            var data = _context.ThuongHieus.OrderBy(t => t.TenThuongHieu).ToList();
            return View("~/Views/Brands/Index.cshtml", data);
        }


        [HttpPost]
        public IActionResult DeleteBrand(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var brand = _context.ThuongHieus.Find(id);
            if (brand != null)
            {
                _context.ThuongHieus.Remove(brand);
                _context.SaveChanges();
            }

            TempData["Success"] = "Đã xóa thương hiệu.";
            return RedirectToAction(nameof(Brands));
        }
        // ====================================================
        // 🏷️ QUẢN LÝ VOUCHER
        // ====================================================
        public IActionResult ManageVouchers()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var vouchers = _context.UuDais.OrderByDescending(u => u.NgayBatDau).ToList();
            return View(vouchers);
        }

        [HttpPost]
        public IActionResult CreateVoucher(string tenUuDai, string moTa, string loaiUuDai,
                                   decimal giaTri, DateTime ngayBatDau,
                                   DateTime ngayKetThuc, int? gioiHanSoLuong)
        {
            if (loaiUuDai != "PhanTram")
            {
                TempData["Error"] = "Chỉ có thể tạo voucher với loại giảm giá là phần trăm.";
                return RedirectToAction("ManageVouchers");
            }

            var uuDai = new UuDai
            {
                TenUuDai = tenUuDai,
                MoTa = moTa,
                LoaiUuDai = loaiUuDai,
                GiaTri = giaTri,
                NgayBatDau = ngayBatDau,
                NgayKetThuc = ngayKetThuc,
                TrangThai = "HoatDong",
                AnhBia = "/images/vouchers/default-voucher.png",
                GioiHanSoLuong = gioiHanSoLuong  // 🔹 lưu giới hạn
            };

            _context.UuDais.Add(uuDai);
            _context.SaveChanges();

            TempData["Success"] = "Voucher đã được tạo thành công!";
            return RedirectToAction("ManageVouchers");
        }




        // Xóa Voucher
        [HttpPost]
        public IActionResult DeleteVoucher(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var voucher = _context.UuDais.Find(id);
            if (voucher != null)
            {
                _context.UuDais.Remove(voucher);
                _context.SaveChanges();
                TempData["Success"] = "Voucher đã được xóa thành công!";
            }

            return RedirectToAction("ManageVouchers");
        }
        // ====================================================
        // 📊 XẾP HẠNG BÀI VIẾT THEO NGƯỜI DÙNG
        // ====================================================
        public IActionResult ThongKeBaiVietNguoiDung()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var danhSach = _context.NguoiDungs
                .Select(u => new ThongKeBaiVietNguoiDung
                {
                    MaNguoiDung = u.MaNguoiDung,
                    TenDangNhap = u.TenDangNhap,
                    HoTen = u.HoTen,
                    TongSoBai = _context.BaiViets.Count(b =>
                        b.MaNguoiDung == u.MaNguoiDung && b.TrangThai != "Đã xóa"),
                    ChoDuyet = _context.BaiViets.Count(b =>
                        b.MaNguoiDung == u.MaNguoiDung && b.TrangThai == "Chờ duyệt"),
                    DangHienThi = _context.BaiViets.Count(b =>
                        b.MaNguoiDung == u.MaNguoiDung &&
                        (b.TrangThai == "Đang hiển thị" || b.TrangThai == "Đã duyệt")),
                    TuChoi = _context.BaiViets.Count(b =>
                        b.MaNguoiDung == u.MaNguoiDung && b.TrangThai == "Từ chối")
                })
                .Where(x => x.TongSoBai > 0)          // chỉ lấy user có bài
                .OrderByDescending(x => x.TongSoBai)  // xếp hạng
                .ToList();

            return View(danhSach);  // Views/Admin/ThongKeBaiVietNguoiDung.cshtml
        }
        // ====================================================
        // 📋 THỐNG KÊ BÀI VIẾT CỦA MỘT NGƯỜI DÙNG
        // ====================================================
        public IActionResult ThongKeBaiViet(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var user = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == id);
            if (user == null) return NotFound();

            var posts = _context.BaiViets
                .Include(b => b.DanhMuc)
                .Where(b => b.MaNguoiDung == id && b.TrangThai != "Đã xóa")
                .OrderByDescending(b => b.NgayTao)
                .ToList();

            var vm = new ThongKeBaiVietNguoiDung
            {
                MaNguoiDung = user.MaNguoiDung,
                TenDangNhap = user.TenDangNhap,
                HoTen = user.HoTen,
                TongSoBai = posts.Count,
                ChoDuyet = posts.Count(b => b.TrangThai == "Chờ duyệt"),
                DangHienThi = posts.Count(b => b.TrangThai == "Đang hiển thị" || b.TrangThai == "Đã duyệt"),
                TuChoi = posts.Count(b => b.TrangThai == "Từ chối"),
                DanhSachBaiViet = posts
            };

            return View(vm);  // Views/Admin/ThongKeBaiViet.cshtml
        }
        // ====================================================
        // 📊 THỐNG KÊ DANH MỤC: NHIỀU BÀI ĐĂNG & ĐƯỢC MUA NHIỀU NHẤT
        // ====================================================
       public IActionResult ThongKeDanhMuc()
{
    if (!IsAdmin())
        return RedirectToAction("Index", "Home");

    var thongKe = _context.DanhMucs
        .Select(dm => new ThongKeDanhMucTongHop
        {
            MaDanhMuc = dm.MaDanhMuc,
            TenDanhMuc = dm.TenDanhMuc,

            // Tổng số bài đăng thuộc danh mục (loại trừ bài đã xóa)
            TongSoBai = _context.BaiViets.Count(b =>
                b.MaDanhMuc == dm.MaDanhMuc &&
                b.TrangThai != "Đã xóa"),

            // 🔹 Tổng số lượt mua theo danh mục
            // Giả định:
            //   ChiTietDonHang: MaDonHang, MaBaiViet
            //   DonHang       : MaDonHang, TrangThai
            TongSoLuotMua = (
                from ctdh in _context.ChiTietDonHangs
                join dh in _context.DonHangs on ctdh.MaDonHang equals dh.MaDonHang
                join bv in _context.BaiViets on ctdh.MaBaiViet equals bv.MaBaiViet
                where bv.MaDanhMuc == dm.MaDanhMuc
                      && dh.TrangThai == "HoanThanh"   // 👉 sửa đúng tên trạng thái hoàn tất của bạn
                select ctdh
            ).Count()
        })
        .OrderByDescending(x => x.TongSoBai)
        .ToList();

    return View(thongKe);
}
        // ====================================================
        // 📦 QUẢN LÝ ĐƠN HÀNG - ADMIN CHỈ XEM / LỌC
        // ====================================================
        public IActionResult Orders(string trangThai, string tuKhoa)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            // DonHang: MaDonHang, NgayDat, TrangThai, TongTien, ...
            // Giả sử có navigation NguoiMua (kiểu NguoiDung)
            var query = _context.DonHangs
                .Include(d => d.NguoiMua) // nếu không có thì bỏ dòng này
                .AsQueryable();

            // Lọc theo trạng thái nếu có chọn
            if (!string.IsNullOrEmpty(trangThai) && trangThai != "ALL")
            {
                query = query.Where(d => d.TrangThai == trangThai);
            }

            // Tìm kiếm theo mã đơn hoặc tên / tên đăng nhập khách
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim().ToLower();

                query = query.Where(d =>
                    d.MaDonHang.ToString().Contains(tuKhoa) ||
                    (d.NguoiMua != null &&
                     (
                        d.NguoiMua.HoTen.ToLower().Contains(tuKhoa) ||
                        d.NguoiMua.TenDangNhap.ToLower().Contains(tuKhoa)
                     )
                    )
                );
            }

            var model = query
                .OrderByDescending(d => d.NgayDat) // đổi lại tên thuộc tính thời gian nếu khác
                .ToList();

            ViewBag.TrangThai = trangThai;
            ViewBag.TuKhoa = tuKhoa;

            return View(model); // Views/Admin/Orders.cshtml
        }
        // ====================================================
        // 📄 CHI TIẾT ĐƠN HÀNG - ADMIN XEM
        // ====================================================
        public IActionResult OrderDetails(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var donHang = _context.DonHangs
                .Include(d => d.NguoiMua)
                .Include(d => d.NguoiBan) // ✅ thêm dòng này
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BaiViet)
                        .ThenInclude(bv => bv.NguoiDung)
                .FirstOrDefault(d => d.MaDonHang == id);

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }



        // ⚠️ ADMIN FORCE HỦY ĐƠN (tuỳ chọn)
        [HttpPost]
        public IActionResult AdminHuyDonHang(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var donHang = _context.DonHangs.FirstOrDefault(d => d.MaDonHang == id);
            if (donHang == null) return NotFound();

            donHang.TrangThai = "DaHuy";   // hoặc "Đã hủy" theo quy ước của bạn
            _context.SaveChanges();

            TempData["Success"] = "Đơn hàng đã được admin hủy (can thiệp khẩn cấp).";

            return RedirectToAction("OrderDetails", new { id });
        }

        public IActionResult BaoCaoTaiKhoanList()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var list = _context.BaoCaoTaiKhoans
                .Include(b => b.NguoiBaoCao)
                .Include(b => b.NguoiBiBaoCao)
                .OrderByDescending(b => b.NgayTao)
                .ToList();

            return View(list); // Views/Admin/BaoCaoTaiKhoanList.cshtml
        }

        public IActionResult ChiTietBaoCaoTaiKhoan(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var bc = _context.BaoCaoTaiKhoans
                .Include(b => b.NguoiBaoCao)
                .Include(b => b.NguoiBiBaoCao)
                .FirstOrDefault(b => b.Id == id);

            if (bc == null) return NotFound();

            return View(bc);
        }

        [HttpPost]
        public IActionResult CapNhatTrangThaiBaoCao(int id, string trangThai)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var bc = _context.BaoCaoTaiKhoans.FirstOrDefault(b => b.Id == id);
            if (bc == null) return NotFound();

            bc.TrangThai = trangThai; // "DangXuLy" / "DaXuLy" ...
            _context.SaveChanges();

            TempData["Success"] = "Đã cập nhật trạng thái báo cáo.";
            return RedirectToAction("ChiTietBaoCaoTaiKhoan", new { id });
        }
        // ====================================================
        // 🏷️ TẠO THƯƠNG HIỆU MỚI
        // ====================================================

        

        // ==========================
        // 🏷️ TẠO THƯƠNG HIỆU MỚI (Không Logo)


        // ====================================================
        // 🏷️ TẠO THƯƠNG HIỆU MỚI (KHÔNG LOGO)
        // ====================================================

        [HttpGet]
        public IActionResult CreateBrand()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            // dùng view đã có: Views/Brands/Create.cshtml
            var model = new ThuongHieu();
            return View("~/Views/Brands/Create.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBrand(ThuongHieu model)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ.";
                // trả lại đúng view trong thư mục Brands
                return View("~/Views/Brands/Create.cshtml", model);
            }

            _context.ThuongHieus.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đã thêm thương hiệu mới thành công!";
            return RedirectToAction(nameof(Brands));   // quay lại trang danh sách brand
        }




    }
}
