using Apps.Auth.Users.Commands;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Server.ChatApp.Protos.Users;

namespace Server.ChatApp.ServiceHandlers.Users;

[Authorize]
public class CommandsHandler(IMediator _mediator) : UserCommandsRPCs.UserCommandsRPCsBase {
    public override async Task<ResultMsg> CreateOrUpdate(Empty request , ServerCallContext context) {
        return ( await _mediator.Send(CreateOrUpdateOnlineUser.New(await SharedMethods.GetMyIdAsync(context , _mediator))) )
            .AsCommonResult();
    }

    public override async Task<ResultMsg> Remove(Empty request , ServerCallContext context) {
        return ( await _mediator.Send(RemoveOnlineUser.New(await SharedMethods.GetMyIdAsync(context , _mediator))) )
           .AsCommonResult();
    }
}
