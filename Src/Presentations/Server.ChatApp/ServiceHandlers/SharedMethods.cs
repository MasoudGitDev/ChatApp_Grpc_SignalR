using Apps.Chats.Shared.Queries;
using Domains.Auth.User.Aggregate;
using Grpc.Core;
using MediatR;
using UnitOfWorks.Abstractions;

namespace Server.ChatApp.ServiceHandlers;
public static class SharedMethods {
    public static async Task<AppUser> GetMyInfoAsync(ServerCallContext context , IChatUOW unitOfWork) {
        var user = context.GetHttpContext().User;
        if(user is null || user.Identity is null || !user.Identity.IsAuthenticated) {
            throw new RpcException(Status.DefaultCancelled , "You are not authenticated.");
        }
        return await unitOfWork.Queries.Users.FindByUserNameAsync(user.Identity.Name ?? string.Empty)
            ?? throw new RpcException(Status.DefaultCancelled , "Invalid-User");
    }
    public static async Task<Guid> GetMyIdAsync(ServerCallContext ctx , IChatUOW unitOfWork) => ( await GetMyInfoAsync(ctx , unitOfWork) ).Id;
    //======================

    public static async Task<AppUser> GetMyInfoAsync(ServerCallContext context , Func<string , Task<AppUser>> findMeByUserName) {
        var user = context.GetHttpContext().User;
        if(user is null || user.Identity is null || !user.Identity.IsAuthenticated) {
            throw new RpcException(Status.DefaultCancelled , "You are not authenticated.");
        }
        return await findMeByUserName.Invoke(user.Identity.Name ?? string.Empty)
            ?? throw new RpcException(Status.DefaultCancelled , "Invalid-User");
    }
    public static async Task<Guid> GetMyIdAsync(ServerCallContext ctx , Func<string , Task<AppUser>> findMeByUserName)
        => ( await GetMyInfoAsync(ctx , findMeByUserName) ).Id;

    //==============
    public static async Task<AppUser> GetMyInfoAsync(ServerCallContext context , IMediator mediator) {
        var user = context.GetHttpContext().User;
        if(user is null || user.Identity is null || !user.Identity.IsAuthenticated) {
            throw new RpcException(Status.DefaultCancelled , "You are not authenticated.");
        }
        return await mediator.Send(FindMeByUserName.New(user.Identity.Name ?? string.Empty))
            ?? throw new RpcException(Status.DefaultCancelled , "Invalid-User");
    }
    public static async Task<Guid> GetMyIdAsync(ServerCallContext ctx , IMediator mediator)
        => ( await GetMyInfoAsync(ctx , mediator) ).Id;
}
