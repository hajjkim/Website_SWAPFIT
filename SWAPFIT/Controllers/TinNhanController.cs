using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Data;

public class TinNhanController : Controller
{
    private readonly ApplicationDbContext _context;

    public TinNhanController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ⭐ TRANG CHAT
    public IActionResult Chat(int nguoiNhanId)
    {
        var userId = HttpContext.Session.GetInt32("MaNguoiDung");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var otherUser = _context.NguoiDungs
            .FirstOrDefault(x => x.MaNguoiDung == nguoiNhanId);

        if (otherUser == null)
            return NotFound();

        ViewBag.OtherUser = otherUser;

        // Lấy toàn bộ lịch sử chat giữa 2 người
        var messages = _context.TinNhans
            .Where(t =>
                (t.MaNguoiGui == userId && t.MaNguoiNhan == nguoiNhanId) ||
                (t.MaNguoiGui == nguoiNhanId && t.MaNguoiNhan == userId)
            )
            .OrderBy(t => t.ThoiGianGui)
            .ToList();

        return View(messages); // Trả về các tin nhắn cho View
    }

    
}
