using Domains.Auth.User.Aggregate;
using Domains.Chats.Shared;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Shared.Server.Extensions;

namespace Server.ChatApp.Hubs.Chats;

public class SharedHubMethods {
    public static async Task<AppUser> GetMyInfoAsync(HubCallerContext context , IChatUOW unitOfWork) {
        var user = context.User;
        if(user is null || user.Identity is null || !user.Identity.IsAuthenticated) {
            throw new RpcException(Status.DefaultCancelled , "You are not authenticated.");
        }
        return await unitOfWork.Queries.Users.FindByUserNameAsync(user.Identity.Name ?? String.Empty)
            ?? AppUser.Empty;
    }
    public static async Task<Guid> GetMyIdAsync(HubCallerContext ctx , IChatUOW unitOfWork) => ( await GetMyInfoAsync(ctx , unitOfWork) ).Id;
    public static Guid GetMyIdByClaims(HubCallerContext ctx) {
        var user = ctx.User;
        if(user is null || user.Identity is null || !user.Identity.IsAuthenticated) {
            return Guid.Empty;
        }
        return user.Claims.Where(x => x.Type == "UserIdentifier").FirstOrDefault()?.Value.AsGuid() ?? Guid.Empty;
    }
}
