using Grpc.Core;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Server.ChatApp.Protos.Users;
using Shared.Server.Dtos.User;
using Shared.Server.Extensions;
using Shared.Server.Models.Results;
using Queries = Apps.Auth.Users.Queries;
using FileDirectory = System.IO.Directory;

namespace Server.ChatApp.ServiceHandlers.Users;

[Authorize]
public class QueriesHandler(IMediator _mediator) : UserQeriesRPCs.UserQeriesRPCsBase {
    public override async Task GetOnlineUsers(Empty request , IServerStreamWriter<OnlineUserMsg> responseStream , ServerCallContext context) {
        ResultStatus<List<UserBasicInfoDto>> result = await _mediator.Send(Queries.GetOnlineUsers.New());
        if(!result.IsSuccessful || result.Model is null) {
            throw new RpcException(Status.DefaultCancelled);
        }
        foreach(var user in result.Model) {
            await responseStream.WriteAsync(user.Adapt<OnlineUserMsg>());
        }
    }

    public override async Task<Protos.File> GetProfileImage(Protos.Directory request , ServerCallContext context) {
        Guid userId = await SharedMethods.GetMyIdAsync(context,_mediator);
        string directory = Path.Combine(FileDirectory.GetCurrentDirectory() , "AccountLogos",userId.ToString());
        // continue later
        return new();
    }

    public override async Task GetUsers(Empty request , IServerStreamWriter<UserBasicInfoMsg> responseStream , ServerCallContext context) {
        ResultStatus<List<UserBasicInfoDto>> result = await _mediator.Send(Queries.GetUsersBasicInfo.New());
        if(!result.IsSuccessful || result.Model is null) {
            throw new RpcException(Status.DefaultCancelled);
        }
        foreach(var user in result.Model) {
            await responseStream.WriteAsync(user.Adapt<UserBasicInfoMsg>());
        }
    }

    public override async Task GetUsersWithOnlineStatus(Empty request , IServerStreamWriter<OnlineUserMsg> responseStream , ServerCallContext context) {
        var result = (await _mediator.Send(Queries.GetUsersWithOnlineStatus.New()));
        if(!result.IsSuccessful || result.Model is null) {
            throw new RpcException(Status.DefaultCancelled);
        }
        foreach(var user in result.Model) {
            await responseStream.WriteAsync(user.Adapt<OnlineUserMsg>());
        }
    }

    public override async Task<ResultMsg> IsOnline(PersonMsg request , ServerCallContext context) {
        return ( await _mediator.Send(Queries.GetOnlineUser.New(request.Id.AsGuid())) ).AsCommonResult();
    }
}
