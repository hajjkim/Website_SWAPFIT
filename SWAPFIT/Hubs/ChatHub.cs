using Microsoft.AspNetCore.SignalR;
using SWAPFIT.Data;
using SWAPFIT.Models;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;

    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task JoinConversation(int me, int other)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, me.ToString());
        await Groups.AddToGroupAsync(Context.ConnectionId, other.ToString());
    }

    public async Task SendMessage(int senderId, int receiverId, string message)
    {
        var chatMessage = new TinNhan
        {
            MaNguoiGui = senderId,
            MaNguoiNhan = receiverId,
            NoiDung = message,
            ThoiGianGui = DateTime.Now
        };

        _context.TinNhans.Add(chatMessage);
        await _context.SaveChangesAsync();

        await Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, message, chatMessage.ThoiGianGui);
    }
}
