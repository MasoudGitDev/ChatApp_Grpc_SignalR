using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Shared.Server.Extensions;
using Commands = Apps.Chats.ChatItems.Commands;

namespace Server.ChatApp.ServiceHandlers.ChatItems;

[Authorize]
internal class ChatItemCommandsHandler(IMediator _mediator) : ChatItemCommandsRPCs.ChatItemCommandsRPCsBase {
    public override async Task<ResultMsg> Create(PersonMsg request , ServerCallContext context)
        => ( await _mediator.Send(Commands.Create.New(await SharedMethods.GetMyIdAsync(context , _mediator) ,
            request.Id.AsGuid())) )
        .AsCommonResult();

    public override async Task<ResultMsg> Remove(RowMsg request , ServerCallContext context)
         => ( await _mediator.Send(Commands.Remove.New(request.RowId.AsGuid())) )
        .AsCommonResult();
}
