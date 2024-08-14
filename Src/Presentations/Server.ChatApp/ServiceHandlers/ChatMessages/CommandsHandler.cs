using Grpc.Core;
using Mapster;
using MediatR;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Server.ChatApp.Protos.ChatMessages;
using Shared.Server.Extensions;
using Commands = Apps.Chats.ChatMessages.Commands;

namespace Server.ChatApp.ServiceHandlers.ChatMessages;

internal class CommandsHandler(IMediator _mediator) : ChatMessageCommandRPCs.ChatMessageCommandRPCsBase {
    public override async Task<ResultMsg> MarkMessagesAsRead(TableMsg request , ServerCallContext context) {
        return ( await _mediator.Send(Commands.MarkMessagesAsRead.New(request.Id.AsGuid())) ).AsCommonResult();
    }

    public override async Task<ResultMsg> Send(SendMessageMsg request , ServerCallContext context) {
        return ( await _mediator.Send(request.Adapt<Commands.Create>()) ).AsCommonResult();
    }
}
