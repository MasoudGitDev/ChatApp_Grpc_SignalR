using Apps.Chats.ChatMessages.Commands;
using Grpc.Core;
using Mapster;
using MediatR;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Server.ChatApp.Protos.ChatMessages;

namespace Server.ChatApp.ServiceHandlers.ChatMessages;

internal class CommandsHandler(IMediator _mediator) : ChatMessageCommandRPCs.ChatMessageCommandRPCsBase {
    public override async Task<ResultMsg> Send(ChatMessageMsg request , ServerCallContext context) {
        return ( await _mediator.Send(request.Adapt<Create>()) ).AsCommonResult();
    }
}
