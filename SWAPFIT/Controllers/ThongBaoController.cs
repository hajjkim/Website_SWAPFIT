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

        var ds = _context.ThongBaos
            .Where(t => t.MaNguoiDung == maNguoiDung.Value)
            .OrderByDescending(t => t.NgayTao)
            .ToList();

        // Đánh dấu đã xem
        foreach (var t in ds.Where(t => !(t.DaXem ?? false)))
            t.DaXem = true;

        _context.SaveChanges();

        return View(ds);
    }

}
