using Microsoft.AspNetCore.SignalR;
using Shared.Server.Dtos.User;

namespace Server.ChatApp.Hubs.Accounts;

public class SignUpHub :Hub{
    public async Task SendNewUser(OnlineUserDto user) {
        await Clients.All.SendAsync("GetNewUser" , user);
    }
}
