using Apps.Chats.UnitOfWorks;
using Domains.Chats.Contacts.Aggregate;
using Domains.Chats.Requests.Aggregate;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Protos;
using Shared.Server.Extensions;

namespace Server.ChatApp.GRPCHandlers;

[Authorize]
public class ChatRequestHandler(IChatUOW _unitOfWork) : ChatRequestRPCs.ChatRequestRPCsBase
{

    public override async Task<ResultMsg> Request(UsersConnection request, ServerCallContext context)
    {
        await _unitOfWork.CreateAsync(ChatRequest.Create(request.RequesterId.AsGuid(), request.ReceiverId.AsGuid()));
        return DefaultResult;
    }

    public override async Task<ResultMsg> Accept(ChatRequestMsg request , ServerCallContext context)
    {
        return await ApplyAsync(request, async (model) =>
        {
            await _unitOfWork.DeleteAsync(model);
            await _unitOfWork.CreateAsync(Contact.Create(model.RequesterId, model.ReceiverId));
        });
    }

    public override async Task<ResultMsg> Block(ChatRequestMsg request , ServerCallContext context)
    {
        return await ApplyAsync(request, async (model) => await model.BlockAsync());
    }

    public override async Task<ResultMsg> Delete(ChatRequestMsg request , ServerCallContext context)
    {
        return await ApplyAsync(request, async (model) => await _unitOfWork.DeleteAsync(model));
    }

    public override async Task<ResultMsg> Unblock(ChatRequestMsg request , ServerCallContext context)
    {
        return await ApplyAsync(request, async (model) => await model.UnBlockAsync());
    }


    //======================= privates

    private async Task<ResultMsg> ApplyAsync(ChatRequestMsg request , Func<ChatRequest, Task> actions)
    {
        var model = await FindRequestAsync(request.ChatRequestId.AsGuid());
        await actions.Invoke(model);
        await _unitOfWork.SaveChangeAsync();
        return DefaultResult;
    }
    private async Task<ChatRequest> FindRequestAsync(Guid chatRequestId)
    {
        var model = await _unitOfWork.Queries.ChatRequests.FindByIdAsync(chatRequestId)
            ?? throw new RpcException(Status.DefaultCancelled, $"The chatRequestId : <{chatRequestId}> not exist.");
        return model;
    }
    private static ResultMsg DefaultResult => new() { IsSuccessful = true };

}
