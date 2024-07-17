using Apps.Chats.Commands;
using Domains.Chats.Message.Aggregate;
using Domains.Chats.Shared;
using Shared.Server.Models.Results;

namespace Apps.Chats.Implementations;

public class ChatMessageCommands(IChatUOW _unitOfWork) : IChatMessageCommands {

    public static ChatMessageCommands Instance(IChatUOW unitOfWork) => new(unitOfWork);

    public async Task<Result> SendAsync(ChatMessage chatMessage) {
        await _unitOfWork.CreateAsync(chatMessage);
        await _unitOfWork.SaveChangeAsync();
        return ChatMessageResult.Send;
    }

    public async Task<Result> DeleteAsync(Guid messageId) {
        var item = await _unitOfWork.Queries.ChatMessages.FindByIdAsync(messageId);
        if(item is null) {
            return ChatMessageResult.NotFound($"The {nameof(messageId)} : <{messageId}> is not founded.");
        }
        await _unitOfWork.DeleteAsync(item);
        await _unitOfWork.SaveChangeAsync();
        return ChatMessageResult.Delete;
    }

}