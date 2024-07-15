using Apps.Chats.ChatItems.Commands.Model;
using Apps.Chats.Shared.Models;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Extensions;
using Server.ChatApp.GRPCHandlers;
using Server.ChatApp.Protos;
using Shared.Server.Extensions;

namespace Server.ChatApp.ServiceHandlers.ChatItems;

[Authorize]
public class ChatItemCommandsHandler(IMediator _mediator) : ChatItemCommandsRPCs.ChatItemCommandsRPCsBase
{
    public override async Task<ResultMsg> Create(PersonMsg request, ServerCallContext context)
        => (await _mediator.Send(new CreateRequest(await GetMyIdAsync(context), request.Id.AsGuid()))).AsCommonResult();

    public override async Task<ResultMsg> Remove(RowMsg request, ServerCallContext context)
         => (await _mediator.Send(new RemoveRequest(request.RowId.AsGuid()))).AsCommonResult();

    private async Task<Guid> GetMyIdAsync(ServerCallContext ctx)
        => await SharedMethods.GetMyIdAsync(ctx,
            async (userName) => await _mediator.Send(new FindMeByUserNameRequest(userName)));
}
