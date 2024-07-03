using Apps.Chats.UnitOfWorks;
using Domains.Chats.Requests.Aggregate;
using Google.Protobuf.Collections;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Protos;

namespace Server.ChatApp.GRPCHandlers;

[Authorize]
internal sealed class ChatRequestQueriesHandler(IChatUOW _unitOfWork) : ChatRequestQueryRPCs.ChatRequestQueryRPCsBase {
    public override async Task<CRQResult> GetReceiveRequests(UserMsg request , ServerCallContext context)
        => await CreateResultAsync(context , _unitOfWork , _unitOfWork.Queries.ChatRequests.GetReceiveRequestsAsync);

    public override async Task<CRQResult> GetSendRequests(UserMsg request , ServerCallContext context)
        => await CreateResultAsync(context , _unitOfWork , _unitOfWork.Queries.ChatRequests.GetSendRequestsAsync);


    //================ privates
    private static async Task<CRQResult> CreateResultAsync(ServerCallContext ctx ,
        IChatUOW _unitOfWork , Func<Guid , Task<LinkedList<ChatRequest>>> action) {
        var result = new CRQResult(){ IsSuccessful = true };
        var userId = await SharedMethods.GetMyIdAsync(ctx , _unitOfWork);
        result.Items.AddRange(ToRepeatedFields(await action.Invoke(userId)));
        return result;
    }
    private static RepeatedField<ChatRequestItemMsg> ToRepeatedFields(LinkedList<ChatRequest> items) {
        var itemsResult = new RepeatedField<ChatRequestItemMsg>();
        foreach(var item in items) {
            itemsResult.Add(item.Adapt<ChatRequestItemMsg>());
        }
        return itemsResult;
    }
}
