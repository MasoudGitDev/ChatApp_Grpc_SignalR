using Apps.Chats.UnitOfWorks;
using Grpc.Core;
using Server.ChatApp.Protos;

namespace Server.ChatApp.GRPCHandlers;

internal sealed class ChatRequestQueriesHandler(IChatUOW _unitOfWork) : ChatRequestQueryRPCs.ChatRequestQueryRPCsBase {
    public override Task<CRQResult> GetReceiveRequests(UserMsg request , ServerCallContext context) {
        return base.GetReceiveRequests(request , context);
    }

    public override Task<CRQResult> GetSendRequests(UserMsg request , ServerCallContext context) {
        return base.GetSendRequests(request , context);
    }
}
