using Domains.Chats.Item.Aggregate;
using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatItems.Commands;

public sealed record Create(Guid RequesterId , Guid ReceiverId) : IRequest<ResultStatus> {
    public static Create New(Guid RequesterId , Guid ReceiverId) => new(RequesterId , ReceiverId);
}

internal sealed class CreateHandler(IChatUOW _unitOfWork) : IRequestHandler<Create , ResultStatus> {
    public async Task<ResultStatus> Handle(Create request , CancellationToken cancellationToken) {
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