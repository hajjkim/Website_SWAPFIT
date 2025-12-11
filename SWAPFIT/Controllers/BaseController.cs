using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SWAPFIT.Data;
using Microsoft.EntityFrameworkCore;

namespace SWAPFIT.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            if (maNguoiDung.HasValue)
            {
                var soLuongGioHang = await _context.ChiTietGioHangs
                    .Where(ct => ct.GioHang.MaNguoiDung == maNguoiDung.Value)
                    .SumAsync(ct => (int?)ct.SoLuong ?? 0);

                var soLuongThongBaoChuaDoc = await _context.ThongBaos
                    .CountAsync(t => t.MaNguoiDung == maNguoiDung.Value && t.DaXem == false);

                var thongBaos = await _context.ThongBaos
                    .Where(t => t.MaNguoiDung == maNguoiDung.Value)
                    .OrderByDescending(t => t.NgayTao)
                    .Take(10)
                    .ToListAsync();

                HttpContext.Items["ThongBaoData"] = (soLuongGioHang, soLuongThongBaoChuaDoc, thongBaos);
            }

            base.OnActionExecuting(context);
        }
    }
}