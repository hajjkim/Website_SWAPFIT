using Microsoft.AspNetCore.Mvc;
using SWAPFIT.Data;

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

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var otherUser = _context.NguoiDungs
            .FirstOrDefault(x => x.MaNguoiDung == nguoiNhanId);

        if (otherUser == null)
            return NotFound();

        ViewBag.OtherUser = otherUser;
        var messages = _context.TinNhans
            .Where(t =>
                (t.MaNguoiGui == userId && t.MaNguoiNhan == nguoiNhanId) ||
                (t.MaNguoiGui == nguoiNhanId && t.MaNguoiNhan == userId)
            )
            .OrderBy(t => t.ThoiGianGui)
            .ToList();

        return View(messages);
    }

    
}
