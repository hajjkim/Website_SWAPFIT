using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SWAPFIT.Data;
using SWAPFIT.Model;
using SWAPFIT.Models;
using System.Diagnostics;
using System.Text;

namespace SWAPFIT.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThanhToanController> _logger;

        public ThanhToanController(ApplicationDbContext context, ILogger<ThanhToanController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Account");

            int userId = maNguoiDung.Value;
            var maDonHangTam = HttpContext.Session.GetString("MaDonHangTam");
            if (string.IsNullOrEmpty(maDonHangTam))
            {
                maDonHangTam = $"SWF-{DateTime.Now:yyyyMMddHHmmss}-{userId}";
                HttpContext.Session.SetString("MaDonHangTam", maDonHangTam);
            }
            ViewBag.MaDonHangTam = maDonHangTam;
            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.BaiViet)
                        .ThenInclude(b => b.AnhBaiViets)
                .FirstOrDefault(g => g.MaNguoiDung == userId);

            var viewModel = new GioHangViewModel();
            decimal tongTien = 0m;

            var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
            if (!string.IsNullOrEmpty(gioHangTamJson))
            {
                var gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(gioHangTamJson);
                viewModel.GioHangTam = gioHangTam;
                tongTien = gioHangTam.Sum(x => x.GiaSanPham * x.SoLuong);
            }
            else if (gioHang != null && gioHang.ChiTietGioHangs.Any())
            {
                viewModel.GioHang = gioHang;
                tongTien = gioHang.ChiTietGioHangs.Sum(ct =>
                    (ct.BaiViet.GiaSanPham ?? 0m) * ct.SoLuong);
            }

            ViewBag.TongTien = tongTien;
            var userVouchers = _context.UserVouchers
                .Where(uv => uv.UserId == userId)
                .Include(uv => uv.Voucher)
                .Where(uv => uv.Voucher != null && uv.Voucher.TrangThai.Trim() == "HoatDong")
                .Select(uv => uv.Voucher)
                .OrderByDescending(v => v.NgayBatDau)
                .ToList();

            ViewBag.UserVouchers = userVouchers;
            List<int> sellerIds = new();

            if (viewModel.GioHangTam != null && viewModel.GioHangTam.Any())
            {
                sellerIds = viewModel.GioHangTam
                    .Select(x => x.MaNguoiBan)
                    .Distinct()
                    .ToList();
            }
            else if (viewModel.GioHang != null && viewModel.GioHang.ChiTietGioHangs.Any())
            {
                sellerIds = viewModel.GioHang.ChiTietGioHangs
                    .Select(ct => ct.BaiViet.MaNguoiDung)
                    .Distinct()
                    .ToList();
            }

            var bankInfos = _context.TaiKhoanNganHang
                .Where(b => sellerIds.Contains(b.MaNguoiDung))
                .ToList();

            ViewBag.SellerIds = sellerIds;
            ViewBag.BankInfos = bankInfos;

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
            public IActionResult XacNhanThanhToan(
                    string diaChiGiaoHang,
                    string phuongThucThanhToan,
                    string phuongThucGiaoHang,
                    decimal tongTien,
                    string voucherId,
                    string maDonHangTam,
                    string noiDungChuyenKhoan )

        {


            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Account");

            int userId = maNguoiDung.Value;

            var nguoiMua = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == userId);
            string tenNguoiMua = nguoiMua?.HoTen ?? nguoiMua?.TenDangNhap ?? $"Người dùng {userId}";

            var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
            var gioHangTam = string.IsNullOrEmpty(gioHangTamJson)
                ? null
                : JsonConvert.DeserializeObject<List<GioHangTamModel>>(gioHangTamJson);

            bool isFromGioHang = false;

            if (gioHangTam == null || !gioHangTam.Any())
            {
                var gioHang = _context.GioHangs
                    .Include(g => g.ChiTietGioHangs)
                        .ThenInclude(ct => ct.BaiViet)
                            .ThenInclude(b => b.AnhBaiViets)
                    .FirstOrDefault(g => g.MaNguoiDung == userId);

                if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                {
                    TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                    return RedirectToAction("Index", "GioHang");
                }

                isFromGioHang = true;

                gioHangTam = gioHang.ChiTietGioHangs.Select(ct => new GioHangTamModel
                {
                    MaBaiViet = ct.MaBaiViet,
                    TenSanPham = ct.BaiViet.TieuDe,
                    GiaSanPham = ct.BaiViet.GiaSanPham ?? 0,
                    SoLuong = ct.SoLuong,
                    MaNguoiBan = ct.BaiViet.MaNguoiDung,
                    MaNguoiMua = userId,
                    AnhSanPham = ct.BaiViet.AnhBaiViets?.FirstOrDefault()?.DuongDan ?? "/images/no-image.png"
                }).ToList();
            }

            var tongTienServer = gioHangTam.Sum(x => x.GiaSanPham * x.SoLuong);

            var now = DateTime.Now;
            decimal discount = 0m;
            int? selectedVoucherId = null;

            if (!string.IsNullOrWhiteSpace(voucherId) && int.TryParse(voucherId, out int vid))
            {
                var voucher = _context.UuDais.FirstOrDefault(v =>
                    v.MaUuDai == vid &&
                    v.TrangThai.Trim() == "HoatDong" &&
                    v.NgayBatDau <= now &&
                    v.NgayKetThuc >= now
                );

                if (voucher == null)
                {
                    TempData["Error"] = "Voucher không hợp lệ hoặc đã hết hạn.";
                    return RedirectToAction("Index", "ThanhToan");
                }

                var userHasVoucher = _context.UserVouchers
                    .Any(uv => uv.UserId == userId && uv.VoucherId == voucher.MaUuDai);

                if (!userHasVoucher)
                {
                    TempData["Error"] = "Bạn không sở hữu voucher này.";
                    return RedirectToAction("Index", "ThanhToan");
                }

                if (voucher.GioiHanSoLuong.HasValue)
                {
                    var claimed = _context.UserVouchers.Count(uv => uv.VoucherId == voucher.MaUuDai);
                    if (claimed >= voucher.GioiHanSoLuong.Value)
                    {
                        TempData["Error"] = "Voucher đã hết lượt sử dụng.";
                        return RedirectToAction("Index", "ThanhToan");
                    }
                }

                selectedVoucherId = voucher.MaUuDai;

                if (voucher.LoaiUuDai == "PhanTram")
                    discount = tongTienServer * voucher.GiaTri / 100m;
                else if (voucher.LoaiUuDai == "TienMat")
                    discount = voucher.GiaTri;

                if (discount > tongTienServer) discount = tongTienServer;
            }

            try
            {
                var groups = gioHangTam.GroupBy(x => x.MaNguoiBan).ToList();

                var groupTotals = groups.ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.GiaSanPham * x.SoLuong)
                );

                foreach (var nhom in groups)
                {
                    var tongNhom = groupTotals[nhom.Key];

                    decimal discountNhom = 0m;
                    if (discount > 0 && tongTienServer > 0)
                    {
                        discountNhom = Math.Round(discount * (tongNhom / tongTienServer), 0); 
                        if (discountNhom > tongNhom) discountNhom = tongNhom;
                    }

                    var donHang = new DonHang
                    {
                        MaNguoiMua = userId,
                        MaNguoiBan = nhom.Key,
                        DiaChiGiaoHang = diaChiGiaoHang,
                        PhuongThucThanhToan = phuongThucThanhToan,
                        PhuongThucGiaoHang = phuongThucGiaoHang,
                        TrangThai = "Chờ xác nhận",
                        NgayDat = DateTime.Now,
                        TongTien = tongNhom - discountNhom
                    };

                    _context.DonHangs.Add(donHang);
                    _context.SaveChanges(); 

                    _context.ThongBaos.Add(new ThongBao
                    {
                        MaNguoiDung = nhom.Key,
                        NoiDung = $"Bạn có đơn hàng mới #{donHang.MaDonHang} từ {tenNguoiMua}.",
                        LienKet = Url.Action("OrderDetails", "Admin", new { id = donHang.MaDonHang }),
                        DaXem = false,
                        NgayTao = DateTime.Now
                    });
                    _context.SaveChanges();

                    foreach (var item in nhom)
                    {
                        _context.ChiTietDonHangs.Add(new ChiTietDonHang
                        {
                            MaDonHang = donHang.MaDonHang,
                            MaBaiViet = item.MaBaiViet,
                            SoLuong = item.SoLuong,
                            Gia = item.GiaSanPham
                        });
                    }
                    _context.SaveChanges();

                    foreach (var item in nhom)
                    {
                        var sanPham = _context.BaiViets.FirstOrDefault(b => b.MaBaiViet == item.MaBaiViet);
                        if (sanPham != null && sanPham.SoLuong >= item.SoLuong)
                        {
                            sanPham.SoLuong -= item.SoLuong;
                            _context.BaiViets.Update(sanPham);
                        }
                        else
                        {
                            TempData["Error"] = $"Sản phẩm {sanPham?.TieuDe} không đủ số lượng!";
                            return RedirectToAction("Index", "GioHang");
                        }
                    }
                    _context.SaveChanges();
                }

                if (selectedVoucherId.HasValue)
                {
                    var usedVoucher = _context.UserVouchers
                        .FirstOrDefault(uv => uv.UserId == userId && uv.VoucherId == selectedVoucherId.Value);

                    if (usedVoucher != null)
                    {
                        _context.UserVouchers.Remove(usedVoucher);
                        _context.SaveChanges();
                    }
                }
                HttpContext.Session.Remove("GioHangTam");

                if (isFromGioHang)
                {
                    var gioHang = _context.GioHangs.FirstOrDefault(g => g.MaNguoiDung == userId);
                    if (gioHang != null)
                    {
                        _context.GioHangs.Remove(gioHang);
                        _context.SaveChanges();
                    }
                }

                TempData["Success"] = "Đặt hàng thành công! Đơn hàng đang chờ người bán xác nhận.";
                return View("ThongBao");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đặt hàng");
                TempData["Error"] = "Có lỗi xảy ra khi đặt hàng! Vui lòng thử lại.";
                return RedirectToAction("Index");
            }
        }



        [HttpGet]
        public IActionResult LayThongTinChuyenKhoan()
        {
            var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
            if (string.IsNullOrEmpty(gioHangTamJson))
                return PartialView("_ThongTinChuyenKhoan", new List<TaiKhoanNganHang>());

            var gioHangTam = JsonConvert.DeserializeObject<List<GioHangTamModel>>(gioHangTamJson);

           
            var sellerIds = gioHangTam
                .Select(x => x.MaNguoiBan)
                .Distinct()
                .ToList();

            
            var banks = _context.TaiKhoanNganHang
                .Where(b => sellerIds.Contains(b.MaNguoiDung))
                .ToList();

            return PartialView("_ThongTinChuyenKhoan", banks);
        }




        public IActionResult MuaNgay(int id)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (!maNguoiDung.HasValue)
                return RedirectToAction("Login", "Account");

            var sanPham = _context.BaiViets
                .Include(b => b.AnhBaiViets)
                .Include(b => b.NguoiDung)
                .FirstOrDefault(b => b.MaBaiViet == id && b.SoLuong > 0);

            if (sanPham == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại hoặc đã hết!";
                return RedirectToAction("Index", "Home");
            }

            
            var gioHangTam = new List<GioHangTamModel>
    {
        new GioHangTamModel
        {
            MaBaiViet = sanPham.MaBaiViet,
            TenSanPham = sanPham.TieuDe,
            GiaSanPham = sanPham.GiaSanPham ?? 0,
            SoLuong = 1,
            MaNguoiBan = sanPham.MaNguoiDung,
            MaNguoiMua= maNguoiDung.Value,

           AnhSanPham = sanPham.AnhBaiViets?.FirstOrDefault()?.DuongDan
              ?? "/images/no-image.png"
        }
    };

            
            HttpContext.Session.SetString("GioHangTam", JsonConvert.SerializeObject(gioHangTam));
                    

            var gioHangTamJson = HttpContext.Session.GetString("GioHangTam");
            if (string.IsNullOrEmpty(gioHangTamJson))
            {
                TempData["Error"] = "Không tìm thấy giỏ hàng tạm!";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "ThanhToan");


            return RedirectToAction("Index", "ThanhToan");
        }


        public IActionResult ThongBao()
        {
            return View();
        }

    }
}
