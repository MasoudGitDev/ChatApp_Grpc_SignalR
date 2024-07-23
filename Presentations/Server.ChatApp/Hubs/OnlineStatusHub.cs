using Apps.Auth.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Server.ChatApp.Hubs;


public class OnlineStatusHub : Hub {

    public async Task SetOnlineStatus(bool isActive) {
        await Clients.All.SendAsync("GetOnlineStatus" , isActive);
    }

    public override async Task OnDisconnectedAsync(Exception? exception) {

    }
}
