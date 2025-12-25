using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SWAPFIT.Data;
using System.Linq;

public class ThongBaoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ThongBaoController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        int? maNguoiDung = _httpContextAccessor.HttpContext?.Session.GetInt32("MaNguoiDung");
        if (!maNguoiDung.HasValue)
            return RedirectToAction("Login", "Account");

        var thongBaos = _context.ThongBaos
            .Where(t => t.MaNguoiDung == maNguoiDung.Value && t.DaXem == false)
            .OrderByDescending(t => t.NgayTao)
            .ToList();

        foreach (var t in thongBaos)
        {
            t.DaXem = true;
        }
        _context.SaveChanges();

        var soLuongThongBaoChuaDoc = _context.ThongBaos
            .Where(t => t.MaNguoiDung == maNguoiDung.Value && t.DaXem == false)
            .Count();

        var soLuongDonHangChuaXuly = _context.DonHangs
            .Where(d => d.MaNguoiMua == maNguoiDung.Value && d.TrangThai == "Chờ xử lý")
            .Count();

        var soLuongBaiVietDuyet = _context.BaiViets
            .Where(b => b.MaNguoiDung == maNguoiDung.Value && b.TrangThai == "Đã duyệt")
            .Count();

        var soLuongBaiVietTuChoi = _context.BaiViets
            .Where(b => b.MaNguoiDung == maNguoiDung.Value && b.TrangThai == "Bị từ chối")
            .Count();

        ViewBag.SLTinNhan = soLuongThongBaoChuaDoc;
        ViewBag.SLDonHang = soLuongDonHangChuaXuly;
        ViewBag.SLBaiVietDuyet = soLuongBaiVietDuyet;
        ViewBag.SLBaiVietTuChoi = soLuongBaiVietTuChoi;

        return View(thongBaos);
    }
}
