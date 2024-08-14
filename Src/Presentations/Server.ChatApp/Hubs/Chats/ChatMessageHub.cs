using Microsoft.AspNetCore.SignalR;
using Shared.Server.Dtos.Chat;
using Shared.Server.Dtos.User;

namespace Server.ChatApp.Hubs.Chats;


public class ChatMessageHub : Hub {
    public async Task SendMessage(GetMessageDto msg) {
        await Clients.All.SendAsync("ReceiveMessage" , msg , new CancellationToken());
    }

    public async Task SendChatItem(UserBasicInfoDto senderInfo , UserBasicInfoDto receiverInfo , Guid chatItemId) {
        await Clients.All.SendAsync("ReceiveChatItem" , senderInfo , receiverInfo , chatItemId , new CancellationToken());
    }
}
