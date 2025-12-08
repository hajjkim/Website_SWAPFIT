using Microsoft.AspNetCore.SignalR;

namespace SWAPFIT.Hubs
{
    public class SessionUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Lấy Session ID của người dùng từ HTTP context
            return connection.GetHttpContext()?.Session.GetInt32("MaNguoiDung")?.ToString();
        }
    }
}
