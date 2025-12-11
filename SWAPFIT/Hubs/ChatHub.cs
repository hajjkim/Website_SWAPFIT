using Microsoft.AspNetCore.SignalR;
using SWAPFIT.Data;
using SWAPFIT.Models;
using System.Threading.Tasks;
<<<<<<< HEAD

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

        // Notify the other user
        await Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, message, chatMessage.ThoiGianGui);
=======
using System;

namespace SWAPFIT.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        // Người dùng join nhóm hội thoại
        public async Task JoinConversation(int user1Id, int user2Id)
        {
            var groupName = GetGroupName(user1Id, user2Id);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Gửi tin nhắn + lưu DB + realtime + thông báo riêng
        public async Task SendMessage(int senderId, int receiverId, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            var tinNhan = new TinNhan
            {
                MaNguoiGui = senderId,
                MaNguoiNhan = receiverId,
                NoiDung = message.Trim(),
                ThoiGianGui = DateTime.Now,
                DaDoc = false
            };

            // Lưu tin nhắn vào cơ sở dữ liệu
            _context.TinNhans.Add(tinNhan);
            await _context.SaveChangesAsync();

            var groupName = GetGroupName(senderId, receiverId);

            // Gửi message cho cả hai người trong cuộc trò chuyện
            await Clients.Group(groupName).SendAsync(
                "ReceiveMessage",
                senderId,
                message,
                tinNhan.ThoiGianGui?.ToString("HH:mm dd/MM")
            );

            // Gửi notification riêng cho người nhận
            await Clients.User(receiverId.ToString()).SendAsync(
                "ReceiveNotification",
                senderId,
                message
            );
        }

        // Lấy tên nhóm của cuộc trò chuyện giữa 2 người dùng
        private string GetGroupName(int user1Id, int user2Id)
        {
            return user1Id < user2Id
                ? $"chat-{user1Id}-{user2Id}"
                : $"chat-{user2Id}-{user1Id}";
        }

        // Khi người dùng ngắt kết nối, sẽ tự động rời nhóm
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Rời khỏi các nhóm khi ngắt kết nối
            await base.OnDisconnectedAsync(exception);
        }
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
    }
}
