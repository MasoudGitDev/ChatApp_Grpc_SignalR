using Apps.Chats.UnitOfWorks;
using Domains.Auth.User.Aggregate;
using Google.Protobuf.Collections;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Protos;
using Shared.Server.Models.Results;

namespace Server.ChatApp.GRPCHandlers;

[Authorize]
internal sealed class ChatRequestQueriesHandler(IChatUOW _unitOfWork) : ChatRequestQueryRPCs.ChatRequestQueryRPCsBase {
    public override async Task<CRQResult> GetReceiveRequests(Empty request , ServerCallContext context)
        => await CreateResultAsync(context , _unitOfWork , _unitOfWork.Queries.ChatRequests.GetReceiveRequestsAsync);

    public override async Task<CRQResult> GetSendRequests(Empty request , ServerCallContext context)
        => await CreateResultAsync(context , _unitOfWork , _unitOfWork.Queries.ChatRequests.GetSendRequestsAsync);


    //================ privates
    private static async Task<CRQResult> CreateResultAsync(ServerCallContext ctx ,
        IChatUOW _unitOfWork , Func<Guid , Task<List<ChatRequestItem>>> action) {
        var result = new CRQResult(){ IsSuccessful = true };
        try {          
            result.Items.AddRange(ToRepeatedFields(
                await action.Invoke(await SharedMethods.GetMyIdAsync(ctx , _unitOfWork))));
            return result;
        }
        catch(Exception e) { 
           result.IsSuccessful = false;
           result.Messages.Add(new MessageInfo() { Code  = "Error" , Description = e.Message });
           return result;
        }
    }
    private static RepeatedField<ChatRequestItemMsg> ToRepeatedFields(List<ChatRequestItem> items) {
        var itemsResult = new RepeatedField<ChatRequestItemMsg>();
        foreach(var item in items) {
            itemsResult.Add(item.Adapt<ChatRequestItemMsg>());
        }
        return itemsResult;
    }
}
