using Apps.Chats.Commands;
using Domains.Chats.Message.Aggregate;
using Domains.Chats.Message.ValueObjects;
using Grpc.Core;
using Server.ChatApp.Protos;
using Shared.Server.Extensions;

namespace Server.ChatApp.GRPCHandlers;

internal class GrpcChatMessageHandler(IChatMessageCommands _messageHandler) : ChatMessageRPCs.ChatMessageRPCsBase {
    public override async Task<ResultMsg> Send(MessageReq request , ServerCallContext context) {
        var (chatId, messageId, senderId, content) = (request.ChatId, request.MessageId, request.SenderId, request.Content);
        FileUrl fileUrl = FileUrl.Create("grpcFileUrl");
        var msg = ChatMessage.Create(chatId.AsGuid(), senderId.AsGuid(), content, fileUrl, messageId.AsGuid());
        await _messageHandler.SendAsync(msg);
        return new ResultMsg() { IsSuccessful = true };
    }
}
