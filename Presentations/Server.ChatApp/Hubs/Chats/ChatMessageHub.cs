using Microsoft.AspNetCore.SignalR;
using Shared.Server.Dtos.Chat;

namespace Apps.Chats.Hubs;


public class ChatMessageHub : Hub {
    public async Task SendMessage(MessageDto msg) {
        await Clients.All.SendAsync("ReceiveMessage" , msg , new CancellationToken());
    }
}
