using Microsoft.AspNetCore.SignalR;
using Shared.Server.Models.Results;

namespace Server.ChatApp.Hubs.Chats;
public class ChatRequestHub : Hub {

    public async Task Request(ChatRequestItem item) {
        await Clients.All.SendAsync("GetReceiveRequests" , item , new CancellationToken());
        await Clients.User(MyId).SendAsync("GetSendRequests" , item , new CancellationToken());
    }
    private string MyId => SharedHubMethods.GetMyIdByClaims(Context).ToString();
}
