using Grpc.Core;
using MediatR;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Server.ChatApp.Protos.ChatMessages;
using Shared.Server.Extensions;
using Messages = Apps.Chats.ChatMessages.Queries;

namespace Server.ChatApp.ServiceHandlers.ChatMessages;

public class QueriesHandler(IMediator _mediator) : ChatMessageQueryRPCs.ChatMessageQueryRPCsBase {
    public override Task<GetChatMessageMsg> GetMessage(TableMsg request , ServerCallContext context) {
        return base.GetMessage(request , context);
    }

    public override async Task<ChatMessageResult> GetMessages(TableMsg request , ServerCallContext context) {
        return ( await _mediator.Send(Messages.GetMessages.New(request.Id.AsGuid())) ).AsChatMessageResult();
    }
}
