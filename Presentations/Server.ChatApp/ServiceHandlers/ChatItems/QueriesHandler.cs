using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Queries = Apps.Chats.ChatItems.Queries;

namespace Server.ChatApp.ServiceHandlers.ChatItems;

[Authorize]
public class QueriesHandler(IMediator _mediator) : ChatItemQueryRPCs.ChatItemQueryRPCsBase {
    public override async Task<ChatItemResultMsg> GetAll(Empty request , ServerCallContext context) {
        return ( await _mediator.Send(Queries.GetChatItems.New(await SharedMethods.GetMyIdAsync(context , _mediator))) )
            .AsChatItemResult();
    }

    public override async Task<ChatItemResultMsg> GetCloudItem(Empty request , ServerCallContext context) {
        return ( await _mediator.Send(Queries.GetCloudItem.New(await SharedMethods.GetMyIdAsync(context , _mediator))) )
            .AsChatItemResult();
    }
}
