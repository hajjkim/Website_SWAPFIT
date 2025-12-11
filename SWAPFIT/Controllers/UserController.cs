<<<<<<< HEAD
﻿
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Mvc;
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Data;
using SWAPFIT.Models;

namespace SWAPFIT.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =============================
        // ⭐ TRANG PROFILE NGƯỜI DÙNG
        // =============================
<<<<<<< HEAD
        //   public IActionResult Index()
        //   {
        //       var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        //       if (userId == null)
        //           return RedirectToAction("Login", "Account");

        //       var user = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == userId);
        //       if (user == null)
        //           return RedirectToAction("Login", "Account");

        //       // 🟢 Bài viết đã đăng
        //       ViewBag.BaiViets = _context.BaiViets
        //           .Include(b => b.AnhBaiViets)
        //           .Where(b => b.MaNguoiDung == userId)
        //           .OrderByDescending(b => b.NgayTao)
        //           .ToList();

        //       // 🟢 Đơn tôi đã mua
        //       ViewBag.DonMua = _context.DonHangs
        //           .Where(d => d.MaNguoiMua == userId)
        //           .Include(d => d.ChiTietDonHangs).ThenInclude(ct => ct.BaiViet)
        //           .Include(d => d.NguoiBan)
        //           .OrderByDescending(d => d.NgayDat)
        //           .ToList();

        //       // 🟢 Đơn người khác mua của tôi
        //       ViewBag.DonBan = _context.DonHangs
        //           .Where(d => d.MaNguoiBan == userId)
        //           .Include(d => d.ChiTietDonHangs).ThenInclude(ct => ct.BaiViet)
        //           .Include(d => d.NguoiMua)
        //           .OrderByDescending(d => d.NgayDat)
        //           .ToList();

        //       //🟢⭐ TIN NHẮN GẦN ĐÂY(20 tin gần nhất)
        //       //ViewBag.TinNhan = _context.TinNhans
        //       //    .Where(t => t.MaNguoiGui == userId || t.MaNguoiNhan == userId)
        //       //    .Include(t => t.NguoiGui)
        //       //    .Include(t => t.NguoiNhan)
        //       //    .OrderByDescending(t => t.ThoiGianGui)
        //       //    .Take(20)
        //       //    .ToList();
        //       ViewBag.TinNhanGanDay =
        //_context.TinNhans
        //    .Where(t => t.MaNguoiGui == userId || t.MaNguoiNhan == userId)
        //    .OrderByDescending(t => t.ThoiGianGui)
        //    .AsEnumerable()
        //    .GroupBy(t => t.MaNguoiGui == userId ? t.MaNguoiNhan : t.MaNguoiGui)
        //    .Select(g =>
        //    {
        //        var tn = g.First(); // tin mới nhất
        //        int otherId = tn.MaNguoiGui == userId ? tn.MaNguoiNhan : tn.MaNguoiGui;

        //        tn.NguoiGui = _context.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == tn.MaNguoiGui);
        //        tn.NguoiNhan = _context.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == tn.MaNguoiNhan);

        //        return tn;
        //    })
        //    .ToList();

        //       // 🟢 Các voucher đã lưu
        //       var claimedVouchers = _context.UserVouchers
        //           .Where(uv => uv.UserId == userId)
        //           .Include(uv => uv.Voucher)  // Include thông tin voucher
        //           .ToList();

        //       // Pass the claimedVouchers to the view
        //       ViewBag.ClaimedVouchers = claimedVouchers;
        //       return View(user);
        //   }



=======
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == userId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // 🟢 Bài viết đã đăng
            ViewBag.BaiViets = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Where(b => b.MaNguoiDung == userId)
                .OrderByDescending(b => b.NgayTao)
                .ToList();

            // 🟢 Đơn tôi đã mua
            ViewBag.DonMua = _context.DonHangs
                .Where(d => d.MaNguoiMua == userId)
                .Include(d => d.ChiTietDonHangs).ThenInclude(ct => ct.BaiViet)
                .Include(d => d.NguoiBan)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            // 🟢 Đơn người khác mua của tôi
            ViewBag.DonBan = _context.DonHangs
                .Where(d => d.MaNguoiBan == userId)
                .Include(d => d.ChiTietDonHangs).ThenInclude(ct => ct.BaiViet)
                .Include(d => d.NguoiMua)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

<<<<<<< HEAD
            //🟢⭐ TIN NHẮN GẦN ĐÂY(20 tin gần nhất)
            ViewBag.TinNhanGanDay =
            _context.TinNhans
                .Where(t => t.MaNguoiGui == userId || t.MaNguoiNhan == userId)
                .OrderByDescending(t => t.ThoiGianGui)
                .AsEnumerable()
                .GroupBy(t => t.MaNguoiGui == userId ? t.MaNguoiNhan : t.MaNguoiGui)
                .Select(g =>
                {
                    var tn = g.First(); // tin mới nhất
                    int otherId = tn.MaNguoiGui == userId ? tn.MaNguoiNhan : tn.MaNguoiGui;

                    tn.NguoiGui = _context.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == tn.MaNguoiGui);
                    tn.NguoiNhan = _context.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == tn.MaNguoiNhan);

                    return tn;
                })
                .ToList();

=======
            // 🟢⭐ TIN NHẮN GẦN ĐÂY (20 tin gần nhất)
            //ViewBag.TinNhan = _context.TinNhans
            //    .Where(t => t.MaNguoiGui == userId || t.MaNguoiNhan == userId)
            //    .Include(t => t.NguoiGui)
            //    .Include(t => t.NguoiNhan)
            //    .OrderByDescending(t => t.ThoiGianGui)
            //    .Take(20)
            //    .ToList();
            ViewBag.TinNhanGanDay =
    _context.TinNhans
        .Where(t => t.MaNguoiGui == userId || t.MaNguoiNhan == userId)
        .OrderByDescending(t => t.ThoiGianGui)
        .AsEnumerable()
        .GroupBy(t => t.MaNguoiGui == userId ? t.MaNguoiNhan : t.MaNguoiGui)
        .Select(g =>
        {
            var tn = g.First(); // tin mới nhất
            int otherId = tn.MaNguoiGui == userId ? tn.MaNguoiNhan : tn.MaNguoiGui;

            tn.NguoiGui = _context.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == tn.MaNguoiGui);
            tn.NguoiNhan = _context.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == tn.MaNguoiNhan);

            return tn;
        })
        .ToList();
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            // 🟢 Các voucher đã lưu
            var claimedVouchers = _context.UserVouchers
                .Where(uv => uv.UserId == userId)
                .Include(uv => uv.Voucher)  // Include thông tin voucher
                .ToList();

            // Pass the claimedVouchers to the view
            ViewBag.ClaimedVouchers = claimedVouchers;
            return View(user);
        }

<<<<<<< HEAD
=======

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        // =============================
        // ⭐ TRANG CÁ NHÂN CÔNG KHAI
        // =============================
        public IActionResult XemTrangCaNhan(int id)
        {
            var nguoiBan = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == id);

            if (nguoiBan == null)
                return NotFound();

            var baiViets = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Where(b => b.MaNguoiDung == id)
                .OrderByDescending(b => b.NgayTao)
                .ToList();

            nguoiBan.BaiViets = baiViets;

            return View(nguoiBan);
        }
        [HttpPost]
        public IActionResult DanhDauDaXem(int id)
        {
            var tb = _context.ThongBaos.Find(id);
            if (tb != null && tb.MaNguoiDung == HttpContext.Session.GetInt32("MaNguoiDung"))
            {
                tb.DaXem = true;
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }
<<<<<<< HEAD
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> BaoCaoNguoiDung(int nguoiBiBaoCaoId, string lyDo, string? moTaChiTiet, List<IFormFile> files)
        //{
        //    var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        //    if (userId == null)
        //        return RedirectToAction("Login", "Account");

        //    if (userId == nguoiBiBaoCaoId)
        //    {
        //        TempData["Error"] = "Bạn không thể tự báo cáo chính mình.";
        //        return RedirectToAction("XemTrangCaNhan", new { id = nguoiBiBaoCaoId });
        //    }

        //    var nguoiBiBaoCao = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == nguoiBiBaoCaoId);
        //    if (nguoiBiBaoCao == null)
        //    {
        //        TempData["Error"] = "Không tìm thấy tài khoản cần báo cáo.";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    var nguoiBaoCao = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == userId.Value);

        //    // Lưu báo cáo
        //    var baoCao = new BaoCaoTaiKhoan
        //    {
        //        MaNguoiBaoCao = userId.Value,
        //        MaNguoiBiBaoCao = nguoiBiBaoCaoId,
        //        LyDo = lyDo,
        //        MoTaChiTiet = moTaChiTiet,
        //        NgayTao = DateTime.Now,
        //        TrangThai = "Moi"
        //    };
        //    _context.BaoCaoTaiKhoans.Add(baoCao);
        //    await _context.SaveChangesAsync();

        //    // Lưu các ảnh làm bằng chứng
        //    if (files != null && files.Count > 0)
        //    {
        //        foreach (var file in files)
        //        {
        //            if (file.Length > 0)
        //            {
        //                var fileName = Path.GetFileName(file.FileName);
        //                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/bcc/", fileName);

        //                try
        //                {
        //                    using (var stream = new FileStream(filePath, FileMode.Create))
        //                    {
        //                        await file.CopyToAsync(stream);
        //                    }

        //                    var baoCaoAnh = new BaoCaoTaiKhoanAnh
        //                    {
        //                        BaoCaoTaiKhoanId = baoCao.Id,
        //                        DuongDan = "/images/bcc/" + fileName
        //                    };

        //                    _context.BaoCaoTaiKhoanAnhs.Add(baoCaoAnh);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"Error while saving file: {ex.Message}");
        //                }
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //    }

        //    // Gửi thông báo cho Admin
        //    var admin = _context.NguoiDungs
        //        .FirstOrDefault(u => u.VaiTro == "Admin" || u.VaiTro == "admin");

        //    if (admin != null)
        //    {
        //        string tenBaoCao = nguoiBaoCao?.HoTen ?? nguoiBaoCao?.TenDangNhap ?? "Người dùng";
        //        string tenBiBaoCao = nguoiBiBaoCao.HoTen ?? nguoiBiBaoCao.TenDangNhap ?? "Không rõ";

        //        var noiDungThongBao = $"{tenBaoCao} đã báo cáo tài khoản {tenBiBaoCao}. Lý do: {lyDo}";
        //        if (!string.IsNullOrWhiteSpace(moTaChiTiet))
        //            noiDungThongBao += $" | Chi tiết: {moTaChiTiet}";

        //        var tb = new ThongBao
        //        {
        //            MaNguoiDung = admin.MaNguoiDung,
        //            NoiDung = noiDungThongBao,
        //            NgayTao = DateTime.Now,
        //            DaXem = false,
        //            LoaiThongBao = "BaoCao"
        //        };

        //        _context.ThongBaos.Add(tb);
        //        await _context.SaveChangesAsync();
        //    }

        //    TempData["Success"] = "Báo cáo đã được gửi thành công. Cảm ơn bạn!";
        //    return RedirectToAction("XemTrangCaNhan", new { id = nguoiBiBaoCaoId });
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BaoCaoNguoiDung(int nguoiBiBaoCaoId, string lyDo, string? moTaChiTiet, List<IFormFile> files)
=======
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BaoCaoNguoiDung(int nguoiBiBaoCaoId, string lyDo, string? moTaChiTiet)
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (userId == nguoiBiBaoCaoId)
            {
                TempData["Error"] = "Bạn không thể tự báo cáo chính mình.";
                return RedirectToAction("XemTrangCaNhan", new { id = nguoiBiBaoCaoId });
            }

            var nguoiBiBaoCao = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == nguoiBiBaoCaoId);
            if (nguoiBiBaoCao == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản cần báo cáo.";
                return RedirectToAction("Index", "Home");
            }

            var nguoiBaoCao = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == userId.Value);
<<<<<<< HEAD
=======

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            // Lưu báo cáo
            var baoCao = new BaoCaoTaiKhoan
            {
                MaNguoiBaoCao = userId.Value,
                MaNguoiBiBaoCao = nguoiBiBaoCaoId,
                LyDo = lyDo,
                MoTaChiTiet = moTaChiTiet,
                NgayTao = DateTime.Now,
                TrangThai = "Moi"
            };
            _context.BaoCaoTaiKhoans.Add(baoCao);
<<<<<<< HEAD
            await _context.SaveChangesAsync();

            // Lưu các ảnh làm bằng chứng
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/bcc/", fileName);
                        try
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            var baoCaoAnh = new BaoCaoTaiKhoanAnh
                            {
                                BaoCaoTaiKhoanId = baoCao.Id,
                                DuongDan = "/images/bcc/" + fileName
                            };
                            _context.BaoCaoTaiKhoanAnhs.Add(baoCaoAnh);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error while saving file: {ex.Message}");
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
=======
            _context.SaveChanges();
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff

            // Gửi thông báo cho Admin
            var admin = _context.NguoiDungs
                .FirstOrDefault(u => u.VaiTro == "Admin" || u.VaiTro == "admin");
<<<<<<< HEAD
=======

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            if (admin != null)
            {
                string tenBaoCao = nguoiBaoCao?.HoTen ?? nguoiBaoCao?.TenDangNhap ?? "Người dùng";
                string tenBiBaoCao = nguoiBiBaoCao.HoTen ?? nguoiBiBaoCao.TenDangNhap ?? "Không rõ";
<<<<<<< HEAD
                var noiDungThongBao = $"{tenBaoCao} đã báo cáo tài khoản {tenBiBaoCao}. Lý do: {lyDo}";
                if (!string.IsNullOrWhiteSpace(moTaChiTiet))
                    noiDungThongBao += $" | Chi tiết: {moTaChiTiet}";
=======

                var noiDungThongBao = $"{tenBaoCao} đã báo cáo tài khoản {tenBiBaoCao}. Lý do: {lyDo}";
                if (!string.IsNullOrWhiteSpace(moTaChiTiet))
                    noiDungThongBao += $" | Chi tiết: {moTaChiTiet}";

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
                var tb = new ThongBao
                {
                    MaNguoiDung = admin.MaNguoiDung,
                    NoiDung = noiDungThongBao,
                    NgayTao = DateTime.Now,
                    DaXem = false,
                    LoaiThongBao = "BaoCao"
                };
<<<<<<< HEAD
                _context.ThongBaos.Add(tb);
                await _context.SaveChangesAsync();
=======

                _context.ThongBaos.Add(tb);
                _context.SaveChanges();
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
            }

            TempData["Success"] = "Báo cáo đã được gửi thành công. Cảm ơn bạn!";
            return RedirectToAction("XemTrangCaNhan", new { id = nguoiBiBaoCaoId });
        }

    }
}
