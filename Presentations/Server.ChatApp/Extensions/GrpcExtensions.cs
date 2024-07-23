using Google.Protobuf.Collections;
using Mapster;
using Server.ChatApp.Protos;
using Server.ChatApp.Protos.Users;
using Shared.Server.Dtos.User;
using Shared.Server.Models;
using Shared.Server.Models.Results;

namespace Server.ChatApp.Extensions;

public static class GrpcExtensions {

    public static RepeatedField<TDestination> ToRepeatedFields<TFrom, TDestination>(this ICollection<TFrom> items)
    where TDestination : class, new()
    where TFrom : class, new() {
        RepeatedField<TDestination> result = [];
        if(items != null && items.Count != 0) {
            foreach(var item in items) {
                result.Add(item.Adapt<TDestination>());
            }
        }
        return result;
    }

    public static ResultMsg AsCommonResult(this IResultStatus result) {
        var resultMsg = new ResultMsg(){ IsSuccessful = result.IsSuccessful };
        resultMsg.Messages.AddRange(result.Messages.ToRepeatedFields<MessageDescription , MessageInfo>());
        return resultMsg;
    }

    public static OnlineUserResultMsg AsOnlineUserResult(this ResultStatus<Guid> result) {
        var resultMsg = new OnlineUserResultMsg(){ IsSuccessful = result.IsSuccessful };
        if(result.Messages.Count > 0) {
            resultMsg.Messages.AddRange(result.Messages.ToRepeatedFields<MessageDescription , MessageInfo>());
        }
        resultMsg.UserId = result.Model.ToString();
        return resultMsg;
    }

    public static CRQResult AsChatRequestQueriesResult(this ResultStatus<List<ChatRequestItem>> result) {
        CRQResult grpcResult = new(){ IsSuccessful = result.IsSuccessful };
        grpcResult.Messages.AddRange(result.Messages.ToRepeatedFields<MessageDescription , MessageInfo>());
        grpcResult.Items.AddRange(result.Model.ToRepeatedFields<ChatRequestItem , ChatRequestItemMsg>());
        return grpcResult;
    }

}
