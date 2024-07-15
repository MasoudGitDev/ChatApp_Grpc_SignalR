using Apps.Chats.ChatItems.Commands.Model;
using Apps.Chats.UnitOfWorks;
using Domains.Chats.Item.Aggregate;
using MediatR;
using Shared.Server.Models.Results;

namespace Apps.Chats.ChatItems.Commands.Handlers;
internal sealed class CreateHandler(IChatUOW _unitOfWork) : IRequestHandler<CreateRequest , ResultStatus> {
    public async Task<ResultStatus> Handle(CreateRequest request , CancellationToken cancellationToken) {
        var (requesterId, receiverId) = request;

        // same id can not have chat!
        if(requesterId == receiverId) {
            return ChatItemResult.SameId;
        }

        //  The Receiver user must exist in the db.
        var receiverUser = await _unitOfWork.Queries.Users.FindByIdAsync(receiverId);
        if(receiverUser is null) {
            return ChatItemResult.NotFound($"The {nameof(receiverId)} : <{receiverId}> is invalid.");
        }

        // ensure each requester-receiver users just have one chatItem row in db that related to each other!
        var isAnyChat = await _unitOfWork.Queries.ChatItems.HaveAnyChatItem(requesterId, receiverId);
        if(isAnyChat) {
            return ChatItemResult.Founded;
        }

        await _unitOfWork.CreateAsync(ChatItem.Create(requesterId , receiverId));
        await _unitOfWork.SaveChangeAsync();
        return ChatItemResult.Created;
    }
}
