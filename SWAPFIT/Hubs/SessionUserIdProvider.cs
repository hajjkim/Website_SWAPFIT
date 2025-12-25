using Microsoft.AspNetCore.SignalR;

namespace SWAPFIT.Hubs
{
    public class SessionUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext()?.Session.GetInt32("MaNguoiDung")?.ToString();
        }
    }
}
