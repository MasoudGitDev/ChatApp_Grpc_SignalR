using Microsoft.AspNetCore.SignalR;
using Shared.Server.Dtos.Chat;

namespace Server.ChatApp.Hubs.Chats;


public class ChatMessageHub : Hub {
    public async Task SendMessage(ChatMessageDto msg) {
        await Clients.All.SendAsync("ReceiveMessage" , msg , new CancellationToken());
    }
}
