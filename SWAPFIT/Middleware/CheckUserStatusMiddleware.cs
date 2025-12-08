using Microsoft.AspNetCore.Http;
using SWAPFIT.Data;
using System.Threading.Tasks;
using System.Linq;

public class CheckUserStatusMiddleware
{
    private readonly RequestDelegate _next;

    public CheckUserStatusMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
    {
        var userId = context.Session.GetInt32("MaNguoiDung");
        if (userId != null)
        {
            var user = db.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == userId);
            if (user != null && user.TrangThai == "Bị khóa")
            {
                // Xóa session, kick user ra login
                context.Session.Clear();
                context.Response.Redirect("/Account/Login?blocked=true");
                return;
            }
        }

        await _next(context);
    }
}
