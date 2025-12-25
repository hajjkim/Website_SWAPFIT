using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Data;
using SWAPFIT.Model;
using SWAPFIT.Models;
using System.Diagnostics;
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
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role != null && role.ToLower() == "admin";
        }
        public IActionResult Dashboard()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

          
            ViewBag.TotalUsers = _context.NguoiDungs.Count();
            ViewBag.TotalPosts = _context.BaiViets.Count();
            ViewBag.TotalCategories = _context.DanhMucs.Count();
            ViewBag.TotalBrands = _context.ThuongHieus.Count();

           
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
                        .Select(u => u.HoTen ?? u.TenDangNhap ?? "Người dùng đã xóa")
                        .FirstOrDefault() ?? "Không xác định"
                })
                .AsEnumerable()
                .Select(x => new BaiViet 
                {
                    MaBaiViet = x.MaBaiViet,
                    TieuDe = x.TieuDe,
                    NgayTao = x.NgayTao,
                    TrangThai = x.TrangThai,
                    NguoiDung = new NguoiDung { HoTen = x.TenNguoiDang }
                })
                .ToList();
            
            var pendingReports = _context.BaoCaoTaiKhoans
     .Include(r => r.NguoiBaoCao)
     .Include(r => r.NguoiBiBaoCao)
     .Where(r => r.TrangThai == "Moi" || r.TrangThai == "Chưa xử lý") 
     .OrderByDescending(r => r.NgayTao)
     .Take(10) 
     .ToList();

            ViewBag.PendingReports = pendingReports;
            return View(pendingPosts);
        }


        

        public IActionResult Posts()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");
            var posts = _context.BaiViets
      .Include(b => b.NguoiDung) 
      .Select(b => new BaiVietDTO
      {
          MaBaiViet = b.MaBaiViet,
          TieuDe = b.TieuDe,
          MaNguoiDung = b.MaNguoiDung,
          HoTen = b.NguoiDung.HoTen,
          TenDangNhap = b.NguoiDung.TenDangNhap,
          GiaSanPham = b.GiaSanPham,
          LoaiBaiDang = b.LoaiBaiDang,
          NgayTao = b.NgayTao,
          TrangThai = b.TrangThai,
          LyDo = b.LyDo,
          MaDanhMuc = b.MaDanhMuc,
          MaDiaChi = b.MaDiaChi,
          MaThuongHieu = b.MaThuongHieu,
          NoiDung = b.NoiDung,
          SoLuong = b.SoLuong
      })
      .OrderByDescending(b => b.NgayTao)
      .ToList();
            foreach (var post in posts)
            {
                Console.WriteLine($"MaBaiViet: {post.MaBaiViet}, TenDangNhap: {post.TenDangNhap}, HoTen: {post.HoTen},manguoidung: {post.MaNguoiDung}");
            }




            return View(posts);  
        }



        public IActionResult PostDetails(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");
            var post = _context.BaiViets
               .Include(b => b.NguoiDung) 
               .Include(b => b.DanhMuc)   
               .Include(b => b.AnhBaiViets) 
               .Where(b => b.MaBaiViet == id)
               .Select(b => new BaiVietDTO
               {
                   MaBaiViet = b.MaBaiViet,
                   TieuDe = b.TieuDe,
                   MaNguoiDung = b.MaNguoiDung,
                   HoTen = b.NguoiDung.HoTen,
                   TenDangNhap = b.NguoiDung.TenDangNhap,
                   GiaSanPham = b.GiaSanPham,
                   LoaiBaiDang = b.LoaiBaiDang,
                   NgayTao = b.NgayTao,
                   TrangThai = b.TrangThai,
                   SoLuong = b.SoLuong,
                   NoiDung = b.NoiDung,
                   TenDanhMuc = b.DanhMuc.TenDanhMuc,
                   LyDo = b.LyDo,
                   MaDanhMuc = b.MaDanhMuc,
                   MaDiaChi = b.MaDiaChi,
                   MaThuongHieu = b.MaThuongHieu,
                   HinhAnhs = b.AnhBaiViets.Select(a => a.DuongDan).ToList() 
               })
               .FirstOrDefault();

            if (post == null)
                return NotFound();


            if (post == null)
                return NotFound();

        

            return View(post); 
        }





       
        public IActionResult Duyet(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

           
            var post = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == id);
            if (post == null)
            {
                TempData["Error"] = "Không tìm thấy bài viết để duyệt.";
                return RedirectToAction(nameof(Dashboard));
            }

           
            string sql = "UPDATE BaiViets SET TrangThai = N'Đang hiển thị', LyDo = NULL WHERE MaBaiViet = {0}";
            var rowsAffected = _context.Database.ExecuteSqlRaw(sql, id);

            if (rowsAffected > 0)
            {
                
                _context.ThongBaos.Add(new ThongBao
                {
                    MaNguoiDung = post.MaNguoiDung,
                    NoiDung = $"Bài viết \"{post.TieuDe}\" đã được duyệt.",
                    LienKet = Url.Action("Index", "User"), 
                    DaXem = false,
                    NgayTao = DateTime.Now
                });

                _context.SaveChanges();
                TempData["Success"] = "Bài viết đã được duyệt!";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy bài viết để duyệt.";
            }

            return RedirectToAction(nameof(Dashboard));
        }


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

            return View(vm);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TuChoi(TuChoiBaiVietViewModel vm)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(vm);

            var post = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == vm.MaBaiViet);
            if (post == null) return NotFound();

            var lyDo = (vm.LyDoTuChoi ?? "").Trim();

            string sqlUpdate = "UPDATE BaiViets SET TrangThai = N'Từ chối', LyDo = {1} WHERE MaBaiViet = {0}";
            var rowsAffected = _context.Database.ExecuteSqlRaw(sqlUpdate, vm.MaBaiViet, lyDo);

            if (rowsAffected == 0)
                return NotFound();

         
            _context.ThongBaos.Add(new ThongBao
            {
                MaNguoiDung = post.MaNguoiDung,
                NoiDung = $"Bài viết \"{post.TieuDe}\" đã bị từ chối. Lý do: {lyDo}",
                LienKet = Url.Action("Index", "User"),
                DaXem = false,
                NgayTao = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Error"] = "Bài viết đã bị từ chối!";
            return RedirectToAction(nameof(Dashboard));
        }




       
        [HttpPost]
      

        public IActionResult DeletePostAdmin(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            
            string sqlUpdate = "UPDATE BaiViets SET TrangThai = N'Đã xóa' WHERE MaBaiViet = {0}";

           
            var rowsAffected = _context.Database.ExecuteSqlRaw(sqlUpdate, id);

            if (rowsAffected == 0)  
                return NotFound();

            TempData["Success"] = "Bài viết đã được chuyển sang trạng thái Đã xóa.";
            return RedirectToAction(nameof(Posts));  
        }


       
        public IActionResult Users()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var users = _context.NguoiDungs
                 .Where(u => u.VaiTro == null || u.VaiTro.ToLower() != "admin") 
                .OrderBy(u => u.VaiTro)
                .ToList();

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

       
        public IActionResult Brands()
        {
            var data = _context.ThuongHieus.OrderBy(t => t.TenThuongHieu).ToList();
            return View("~/Views/Brands/Index.cshtml", data);
        }
        
        public IActionResult CreateBrand()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBrand(ThuongHieu brand)
        {
            if (ModelState.IsValid)
            {
                _context.ThuongHieus.Add(brand);
                _context.SaveChanges();
                TempData["Success"] = "Thêm thương hiệu thành công!";
                return RedirectToAction(nameof(Brands));
            }
            return View(brand);
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
        
        public IActionResult BaoCaoTaiKhoan()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var baoCaoTaiKhoans = _context.BaoCaoTaiKhoans
                .Where(b => b.TrangThai != "Đã xử lý")  
                .OrderByDescending(b => b.NgayTao)
                .ToList();

            return View(baoCaoTaiKhoans);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBaoCao(int userId, string lyDo)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var baoCao = new BaoCaoTaiKhoan
            {
                MaNguoiBiBaoCao = userId,
                LyDo = lyDo,
                NgayTao = DateTime.Now,
                TrangThai = "Chưa xử lý"  
            };

            _context.BaoCaoTaiKhoans.Add(baoCao);
            _context.SaveChanges();

            TempData["Success"] = "Báo cáo tài khoản đã được gửi thành công!";
            return RedirectToAction(nameof(BaoCaoTaiKhoan));
        }

        public IActionResult DuyetBaoCao(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var baoCao = _context.BaoCaoTaiKhoans.Find(id);
            if (baoCao == null)
                return NotFound();

            baoCao.TrangThai = "Đã xử lý";
            _context.SaveChanges();

            TempData["Success"] = "Báo cáo tài khoản đã được xử lý!";
            return RedirectToAction(nameof(Dashboard));
        }

        public IActionResult TuChoiBaoCao(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var baoCao = _context.BaoCaoTaiKhoans.Find(id);
            if (baoCao == null)
                return NotFound();

            baoCao.TrangThai = "Từ chối";
            _context.SaveChanges();

            TempData["Error"] = "Báo cáo tài khoản đã bị từ chối!";
            return RedirectToAction(nameof(Dashboard));
        }
        public IActionResult ChiTietBaoCaoTaiKhoan(int id)
        {
            Debug.WriteLine($"Fetching report details for ID: {id}");

            if (!IsAdmin())
            {
                Debug.WriteLine("User is not admin, redirecting to Home.");
                return RedirectToAction("Index", "Home");
            }

            
            var report = _context.BaoCaoTaiKhoans
                .Include(r => r.NguoiBaoCao) 
                .Include(r => r.NguoiBiBaoCao) 
                .Include(r => r.BaoCaoTaiKhoanAnhs) 
                .FirstOrDefault(r => r.Id == id);

            if (report == null)
            {
                Debug.WriteLine("Report not found for the given ID.");
                return NotFound();
            }
            Debug.WriteLine($"Report found: {report.LyDo} - Reported by: {report.NguoiBaoCao.HoTen}, Reported user: {report.NguoiBiBaoCao.HoTen}");

            Debug.WriteLine($"Number of attached images: {report.BaoCaoTaiKhoanAnhs?.Count ?? 0}");

            if (report.BaoCaoTaiKhoanAnhs != null && report.BaoCaoTaiKhoanAnhs.Any())
            {
                foreach (var anh in report.BaoCaoTaiKhoanAnhs)
                {
                    Debug.WriteLine($"Image URL: {anh.DuongDan}");
                }
            }
            else
            {
                Debug.WriteLine("No images found.");
            }

            return View(report);
        }

       
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
                .Where(x => x.TongSoBai > 0)          
                .OrderByDescending(x => x.TongSoBai)  
                .ToList();

            return View(danhSach);  
        }
       
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

            return View(vm);  
        }
       
        public IActionResult ThongKeDanhMuc()
        {
            var thongKe = _context.DanhMucs
                .Select(dm => new ThongKeDanhMucTongHop
                {
                    MaDanhMuc = dm.MaDanhMuc,
                    TenDanhMuc = dm.TenDanhMuc ?? "Chưa đặt tên",

                    TongSoBai = _context.BaiViets
                        .Count(bv => bv.MaDanhMuc == dm.MaDanhMuc && bv.TrangThai != "Đã xóa"),

                  
                    TongSoLuotMua = _context.ChiTietDonHangs
                        .Where(ctdh =>
                            ctdh.BaiViet != null &&
                            ctdh.BaiViet.MaDanhMuc == dm.MaDanhMuc &&
                            ctdh.BaiViet.TrangThai != "Đã xóa" &&
                            ctdh.DonHang != null &&
                            ctdh.DonHang.TrangThai != null &&
                            (
                                ctdh.DonHang.TrangThai == "Đang giao" ||
                                ctdh.DonHang.TrangThai == "Đã hoàn thành"
                            )
                        )
                        .Count()
                })
                .OrderByDescending(x => x.TongSoBai)
                .ThenByDescending(x => x.TongSoLuotMua)
                .ToList();

            return View(thongKe);
        }


        //public IActionResult Orders()
        //{
        //    if (!IsAdmin())
        //        return RedirectToAction("Index", "Home");

        //    // Lấy danh sách đơn hàng với thông tin cơ bản
        //    var orders = _context.DonHangs
        //        // Bao gồm thông tin người mua và người bán (navigation properties)
        //        .Include(o => o.NguoiMua)
        //        .Include(o => o.NguoiBan)
        //        // Bao gồm chi tiết đơn hàng và thông tin bài viết
        //        .Include(o => o.ChiTietDonHangs)
        //            .ThenInclude(ct => ct.BaiViet)
        //        // Sắp xếp theo ngày đặt
        //        .OrderByDescending(o => o.NgayDat)
        //        .ToList();

        //    // Trả về view với danh sách đơn hàng
        //    return View(orders);
        //}

        public IActionResult Orders(string trangThai = "ALL", string tuKhoa = "")
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            // Lấy danh sách đơn hàng với thông tin cơ bản
            var query = _context.DonHangs
                // Bao gồm thông tin người mua và người bán (navigation properties)
                .Include(o => o.NguoiMua)
                .Include(o => o.NguoiBan)
                // Bao gồm chi tiết đơn hàng và thông tin bài viết
                .Include(o => o.ChiTietDonHangs)
                    .ThenInclude(ct => ct.BaiViet)
                // Sắp xếp theo ngày đặt
                .OrderByDescending(o => o.NgayDat)
                .AsQueryable();

            // Lọc theo trạng thái nếu có
            if (trangThai != "ALL")
            {
                query = query.Where(o => o.TrangThai == trangThai);
            }

            // Lọc theo từ khóa nếu có
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(o => o.MaDonHang.ToString().Contains(tuKhoa) || o.NguoiMua.HoTen.Contains(tuKhoa));
            }

            // Truy vấn dữ liệu
            var orders = query.ToList();

            // Trả về view với danh sách đơn hàng và các filter
            ViewBag.TrangThai = trangThai;
            ViewBag.TuKhoa = tuKhoa;

            return View(orders);
        }


        public IActionResult OrderDetails(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            // Lấy thông tin chi tiết của đơn hàng
            var order = _context.DonHangs
                .Include(o => o.NguoiMua)  // Bao gồm thông tin người mua
                .Include(o => o.NguoiBan)  // Bao gồm thông tin người bán
                .Include(o => o.ChiTietDonHangs)  // Bao gồm chi tiết đơn hàng
                    .ThenInclude(ct => ct.BaiViet)  // Bao gồm thông tin bài viết (sản phẩm)
                    .ThenInclude(bv => bv.AnhBaiViets)  // Bao gồm ảnh bài viết (sản phẩm)
                .FirstOrDefault(o => o.MaDonHang == id);

            if (order == null)
                return NotFound();  // Nếu không tìm thấy đơn hàng, trả về lỗi NotFound

            return View(order);  // Trả về view với thông tin chi tiết đơn hàng
        }



        // Chi tiết đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeOrderStatus(int id, string newStatus)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var order = _context.DonHangs.Find(id);
            if (order == null)
                return NotFound();

            // Cập nhật trạng thái đơn hàng
            order.TrangThai = newStatus;
            _context.SaveChanges();

            TempData["Success"] = "Trạng thái đơn hàng đã được cập nhật!";
            return RedirectToAction(nameof(Orders));  // Quay lại trang danh sách đơn hàng
        }




        // Xóa đơn hàng (Soft delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOrder(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var order = _context.DonHangs
                .Include(o => o.ChiTietDonHangs)  // Bao gồm các chi tiết đơn hàng
                .FirstOrDefault(o => o.MaDonHang == id);

            if (order == null)
                return NotFound();

            // Cập nhật trạng thái đơn hàng thành "Đã xóa"
            order.TrangThai = "Đã xóa";
            _context.SaveChanges();

            TempData["Success"] = "Đơn hàng đã được xóa.";
            return RedirectToAction(nameof(Orders));  // Quay lại trang danh sách đơn hàng
        }


        // Xem báo cáo đơn hàng theo người dùng
        public IActionResult OrderReportByUser()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var report = _context.NguoiDungs
                .Select(u => new OrderReport
                {
                    MaNguoiDung = u.MaNguoiDung,
                    TenDangNhap = u.TenDangNhap,
                    HoTen = u.HoTen,
                    TongSoDon = _context.DonHangs.Count(d => d.MaNguoiMua == u.MaNguoiDung && d.TrangThai != "Đã xóa"),
                    DonHienTai = _context.DonHangs.Count(d => d.MaNguoiMua == u.MaNguoiDung && d.TrangThai == "Đang xử lý"),
                    DonHoanThanh = _context.DonHangs.Count(d => d.MaNguoiMua == u.MaNguoiDung && d.TrangThai == "Hoàn thành"),
                    DonHuy = _context.DonHangs.Count(d => d.MaNguoiMua == u.MaNguoiDung && d.TrangThai == "Đã hủy"),
                    TongTien = _context.DonHangs.Where(d => d.MaNguoiMua == u.MaNguoiDung).Sum(d => d.TongTien)
                })
                .OrderByDescending(x => x.TongSoDon)
                .ToList();

            return View(report); // Hiển thị báo cáo đơn hàng theo người dùng
        }


        // Thống kê đơn hàng theo trạng thái
        public IActionResult OrderReportByStatus()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var report = _context.DonHangs
                .GroupBy(d => d.TrangThai)
                .Select(g => new OrderStatusReport
                {
                    TrangThai = g.Key,
                    SoLuong = g.Count(),
                    TongTien = g.Sum(d => d.TongTien)
                })
                .ToList();

            return View(report);  // Hiển thị báo cáo theo trạng thái đơn hàng
        }
        // ====================================================
        // 🎫 QUẢN LÝ VOUCHER / ƯU ĐÃI
        // ====================================================

        // Danh sách tất cả voucher
        public IActionResult Vouchers()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var list = _context.UuDais
                .OrderByDescending(v => v.NgayBatDau)
                .ToList();

            // Chỉ rõ tên view
            return View("ManageVouchers", list);
        }


        // ===== TẠO VOUCHER =====
        [HttpGet]
        public IActionResult CreateVoucher()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            // View dùng model UuDai
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateVoucher(
    string tenUuDai,
    string moTa,
    string loaiUuDai,
    decimal giaTri,
    DateTime ngayBatDau,
    DateTime ngayKetThuc,
    int? gioiHanSoLuong)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var uuDai = new UuDai
            {
                TenUuDai = tenUuDai,
                MoTa = moTa,
                LoaiUuDai = loaiUuDai,
                GiaTri = giaTri,
                NgayBatDau = ngayBatDau,
                NgayKetThuc = ngayKetThuc,
                GioiHanSoLuong = gioiHanSoLuong,
                TrangThai = "HoatDong",

                // 👇 THÊM DÒNG NÀY (hoặc path nào bạn muốn)
                AnhBia = "/images/vouchers/default.png"
            };

            _context.UuDais.Add(uuDai);
            _context.SaveChanges();

            TempData["Success"] = "Đã tạo voucher mới!";
            return RedirectToAction(nameof(ManageVouchers));
        }

        // ===== SỬA VOUCHER =====
        [HttpGet]
        public IActionResult EditVoucher(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var voucher = _context.UuDais.FirstOrDefault(v => v.MaUuDai == id);
            if (voucher == null) return NotFound();

            // View: Views/Admin/EditVoucher.cshtml (model UuDai)
            return View(voucher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditVoucher(UuDai model)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(model);

            var voucher = _context.UuDais.FirstOrDefault(v => v.MaUuDai == model.MaUuDai);
            if (voucher == null) return NotFound();

            voucher.TenUuDai = model.TenUuDai;
            voucher.MoTa = model.MoTa;
            voucher.LoaiUuDai = model.LoaiUuDai;
            voucher.GiaTri = model.GiaTri;
            voucher.NgayBatDau = model.NgayBatDau;
            voucher.NgayKetThuc = model.NgayKetThuc;
            voucher.TrangThai = model.TrangThai;
            voucher.GioiHanSoLuong = model.GioiHanSoLuong;   // nếu có cột này
            // bổ sung các thuộc tính khác nếu UuDai có

            _context.SaveChanges();

            TempData["Success"] = "Đã cập nhật voucher!";
            return RedirectToAction(nameof(Vouchers));
        }

        // ===== BẬT / TẮT TRẠNG THÁI (HoatDong / Ngung) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleVoucherStatus(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var voucher = _context.UuDais.FirstOrDefault(v => v.MaUuDai == id);
            if (voucher == null) return NotFound();

            voucher.TrangThai = voucher.TrangThai == "HoatDong" ? "Ngung" : "HoatDong";
            _context.SaveChanges();

            TempData["Success"] = "Đã cập nhật trạng thái voucher!";
            return RedirectToAction(nameof(Vouchers));
        }

        // ===== XÓA VOUCHER =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteVoucher(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var voucher = _context.UuDais.FirstOrDefault(v => v.MaUuDai == id);
            if (voucher == null) return NotFound();

            _context.UuDais.Remove(voucher);
            _context.SaveChanges();

            TempData["Success"] = "Đã xóa voucher!";
            return RedirectToAction(nameof(Vouchers));
        }
        public IActionResult ManageVouchers()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            // Redirect nội bộ sang action Vouchers trong chính AdminController
            return RedirectToAction(nameof(Vouchers));
        }
        public IActionResult Category()
        {
            return View("~/Views/Category/Create.cshtml");
        }


        public IActionResult Create()
        {
            // Your code to handle Create view
            return View();
        }
    }
    }
