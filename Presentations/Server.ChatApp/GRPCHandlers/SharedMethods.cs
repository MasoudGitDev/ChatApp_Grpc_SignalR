using Apps.Chats.UnitOfWorks;
using Domains.Auth.User.Aggregate;
using Grpc.Core;

namespace Server.ChatApp.GRPCHandlers;  
public static class SharedMethods {
    public static async Task<AppUser> GetMyInfoAsync(ServerCallContext context , IChatUOW unitOfWork) {
        var user = context.GetHttpContext().User;
        if(user is null || user.Identity is null || !user.Identity.IsAuthenticated) {
            throw new RpcException(Status.DefaultCancelled , "You are not authenticated.");
        }
        return await unitOfWork.Queries.Users.FindByUserNameAsync(user.Identity.Name ?? String.Empty)
            ?? throw new RpcException(Status.DefaultCancelled , "Invalid-User");
    }
    public static async Task<Guid> GetMyIdAsync(ServerCallContext ctx , IChatUOW unitOfWork) => ( await GetMyInfoAsync(ctx,unitOfWork) ).Id;
    //======================
}
