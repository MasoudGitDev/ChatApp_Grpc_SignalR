using Microsoft.AspNetCore.SignalR;
using Shared.Server.Dtos.User;

namespace Server.ChatApp.Hubs;


public class OnlineStatusHub : Hub {

    public async Task SetOnlineStatus(string userId , bool isActive) {
        await Clients.All.SendAsync("GetOnlineStatus" , userId , isActive);
    }

    public async Task SendOnlineUserInfo(OnlineUserDto user) {
        await Clients.All.SendAsync("GetOnlineUserInfo" , user);
    }
}
