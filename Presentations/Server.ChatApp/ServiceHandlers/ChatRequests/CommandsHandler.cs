using Commands = Apps.Chats.ChatRequests.Commands;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Extensions;
using Server.ChatApp.Protos;
using Shared.Server.Extensions;

namespace Server.ChatApp.ServiceHandlers.ChatRequests;

/// <summary>
/// A ChatRequest Commands Handler
/// </summary>
[Authorize]
internal class CommandsHandler(IMediator _mediator) : ChatRequestCommandsRPCs.ChatRequestCommandsRPCsBase {

    private readonly CancellationToken _cancellationToken = new();

    public override async Task<ResultMsg> Accept(ChatRequestMsg request , ServerCallContext context)
        => ( await _mediator.Send(Commands.Accept.New(request.ChatRequestId.AsGuid() ,
            await GetMyIdAsync(context)) , _cancellationToken) )
        .AsCommonResult();

    public override async Task<ResultMsg> Block(ChatRequestMsg request , ServerCallContext context)
        => ( await _mediator.Send(Commands.Block.New(
            request.ChatRequestId.AsGuid() , await GetMyIdAsync(context)) , _cancellationToken) )
        .AsCommonResult();

    public override async Task<ResultMsg> Delete(ChatRequestMsg request , ServerCallContext context)
        => ( await _mediator.Send(Commands.Remove.New(request.ChatRequestId.AsGuid()) ,
            _cancellationToken) )
        .AsCommonResult();

    public override async Task<ResultMsg> Request(PersonMsg request , ServerCallContext context)
        => ( await _mediator.Send(Commands.Create.New(
            await GetMyIdAsync(context) , request.Id.AsGuid()) , _cancellationToken) )
        .AsCommonResult();

    public override async Task<ResultMsg> Unblock(ChatRequestMsg request , ServerCallContext context)
        => ( await _mediator.Send(Commands.Unblock.New(
             request.ChatRequestId.AsGuid() , await GetMyIdAsync(context)) , _cancellationToken) )
        .AsCommonResult();

    private async Task<Guid> GetMyIdAsync(ServerCallContext ctx)
        => await SharedMethods.GetMyIdAsync(ctx , _mediator);
}
