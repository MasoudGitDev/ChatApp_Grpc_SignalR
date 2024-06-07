using Domains.Chat.ChatAggregate;
using Shared.Server.Extensions;
using Shared.Server.Models;
using Shared.Server.Models.Results;

namespace Apps.Chats.Commands.Impls;

public class ChatItemCommands(IChatUOW _unitOfWork) : IChatItemCommands
{

    public static ChatItemCommands Instance(IChatUOW chatUOW) => new(chatUOW);

    public async Task<Result> CreateAsync(Guid requesterId, Guid receiverId)
    {

        // same id can not have chat!
        if (requesterId == receiverId)
        {
            return ChatItemResult.SameId;
        }

        //  The Receiver user must exist in the db.
        (await _unitOfWork.UserQueries.FindByIdAsync(receiverId))
            .ThrowIfNull($"The {nameof(receiverId)} : <{receiverId}> is invalid.");

        // ensure each requester-receiver users just have one chatItem row in db that related to each other!
        var isAnyChat = await _unitOfWork.ChatItemQueries.HaveAnyChatItem(requesterId, receiverId);
        if (isAnyChat)
        {
            return ChatItemResult.Duplicate;
        }

        await _unitOfWork.CreateAsync(ChatItem.Create(requesterId, receiverId));
        await _unitOfWork.SaveChangeAsync();
        return ChatItemResult.Created;
    }

    public async Task<Result> DeleteAsync(Guid chatId)
    {
        var chatItem = await _unitOfWork.ChatItemQueries.FindByIdAsync(chatId);
        if (chatItem is null)
        {
            return ChatItemResult.NotFound($"The {nameof(chatId)} : <{chatId}> not found.");
        }
        await _unitOfWork.DeleteAsync(chatItem);
        await _unitOfWork.SaveChangeAsync();
        return Result.Ok;
    }


}
