using Domains.Chats.Item.Aggregate;
using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatItems.Commands;

public sealed record Create(Guid RequesterId , Guid ReceiverId) : IRequest<ResultStatus> {
    public static Create New(Guid RequesterId , Guid ReceiverId) => new(RequesterId , ReceiverId);
}

//============================== handler
internal sealed class CreateHandler(IChatUOW _unitOfWork) : IRequestHandler<Create , ResultStatus> {
    public async Task<ResultStatus> Handle(Create request , CancellationToken cancellationToken) {
        var (requesterId, receiverId) = request;

        // Same-Id-Error!
        if(requesterId == receiverId) {
            return ErrorResults.Canceled("You can not chat with yourself!");
        }

        //  The Receiver user must exist in the chat database.
        var receiverUser = await _unitOfWork.Queries.Users.FindByIdAsync(receiverId);
        if(receiverUser is null) {
            return ErrorResults.NotFound($"The receiver with ID : <{receiverId}> Not Found.");
        }

        //Ensure each requester-receiver pair has exactly one ChatItem row in the chat database.
        var item = await _unitOfWork.Queries.ChatItems.FindByIdsAsync(requesterId, receiverId);
        if(item is not null) {
            return ErrorResults.Founded($"The <Chat-Item> with ID : <{item.Id}> was found.");
        }

        await _unitOfWork.CreateAsync(ChatItem.Create(requesterId , receiverId));
        await _unitOfWork.SaveChangeAsync();
        return SuccessResults.Ok("The new Chat-Item has been created successfully.");
    }
}