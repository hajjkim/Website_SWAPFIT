using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWAPFIT.Data;
using SWAPFIT.Models;
using System;
using System.Linq;

public class TinNhanController : Controller
{
    private readonly ApplicationDbContext _context;

    public TinNhanController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Chat(int nguoiNhanId)
    {
        var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        if (userId == null) return RedirectToAction("Login", "Account");

        var otherUser = _context.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == nguoiNhanId);
        if (otherUser == null) return NotFound();

        ViewBag.OtherUser = otherUser;

        var messages = _context.TinNhans
            .Where(t =>
                (t.MaNguoiGui == userId.Value && t.MaNguoiNhan == nguoiNhanId) ||
                (t.MaNguoiGui == nguoiNhanId && t.MaNguoiNhan == userId.Value)
            )
            .OrderBy(t => t.ThoiGianGui)
            .ToList();

        var unread = _context.TinNhans
            .Where(t => t.MaNguoiGui == nguoiNhanId && t.MaNguoiNhan == userId.Value && t.DaDoc != true)
            .ToList();

        if (unread.Any())
        {
            foreach (var m in unread) m.DaDoc = true;
            _context.SaveChanges();
        }

        return View(messages);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult GuiTinNhan(int nguoiNhanId, string noiDung)
    {
        var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        if (userId == null) return RedirectToAction("Login", "Account");

        noiDung = (noiDung ?? "").Trim();
        if (string.IsNullOrWhiteSpace(noiDung))
            return RedirectToAction("Chat", new { nguoiNhanId });

        var tn = new TinNhan
        {
            MaNguoiGui = userId.Value,
            MaNguoiNhan = nguoiNhanId,
            NoiDung = noiDung,
            ThoiGianGui = DateTime.Now,
            DaDoc = false
        };
        _context.TinNhans.Add(tn);

        var nguoiGui = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == userId.Value);
        string tenNguoiGui = nguoiGui?.HoTen ?? nguoiGui?.TenDangNhap ?? $"Người dùng {userId.Value}";

        var oldNoti = _context.ThongBaos
            .Where(t => t.MaNguoiDung == nguoiNhanId
                        && t.DaXem != true
                        && t.LienKet != null
                        && t.LienKet.Contains($"/TinNhan/Chat?nguoiNhanId={userId.Value}"))
            .ToList();
        if (oldNoti.Any()) _context.ThongBaos.RemoveRange(oldNoti);

        _context.ThongBaos.Add(new ThongBao
        {
            MaNguoiDung = nguoiNhanId,
            NoiDung = $"Bạn có tin nhắn mới từ {tenNguoiGui}",
            LienKet = Url.Action("Chat", "TinNhan", new { nguoiNhanId = userId.Value }),
            DaXem = false,
            NgayTao = DateTime.Now
        });

        _context.SaveChanges();

        return RedirectToAction("Chat", new { nguoiNhanId });
    }
}
