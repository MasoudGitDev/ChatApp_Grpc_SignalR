using Apps.Chats.ChatRequests.Queries;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;

namespace Server.ChatApp.ServiceHandlers.ChatRequests;

[Authorize]
internal class QueriesHandler(IMediator _mediator) : ChatRequestQueryRPCs.ChatRequestQueryRPCsBase {
    public override async Task<CRQResult> GetReceiveRequests(Empty request , ServerCallContext context)
        => ( await _mediator.Send(new GetReceiveRequests(await SharedMethods.GetMyIdAsync(context , _mediator))) )
        .AsChatRequestQueriesResult();

    public override async Task<CRQResult> GetSendRequests(Empty request , ServerCallContext context)
        => ( await _mediator.Send(new GetSendRequests(await SharedMethods.GetMyIdAsync(context , _mediator))) )
        .AsChatRequestQueriesResult();
}
