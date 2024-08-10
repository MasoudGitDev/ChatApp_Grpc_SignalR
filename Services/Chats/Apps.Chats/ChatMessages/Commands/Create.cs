using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.Aggregate;
using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatMessages.Commands;
public sealed record class Create : IRequest<ResultStatus> {

    public Guid Id { get; private set; }
    public Guid ChatItemId { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid ReceiverId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public string FileUrl { get; private set; } = string.Empty;

}


//================================= handler
internal sealed class CreateMessageHandler(IChatUOW _unitOfWork) : IRequestHandler<Create , ResultStatus> {
    public async Task<ResultStatus> Handle(Create request , CancellationToken cancellationToken) {
        try {
            var chatItem = await _unitOfWork.Queries.ChatItems.FindByIdsAsync(request.SenderId,request.ReceiverId);
            // create chatItem if not exist
            if(chatItem is null) {
                chatItem = ChatItem.Create(request.SenderId , request.ReceiverId);
                await _unitOfWork.CreateAsync<ChatItem>(chatItem);
            }

            if(chatItem.IsBlockedByRequester) {
                return ErrorResults.Canceled($"You have been blocked from messaging");
            }
            if(chatItem.IsBlockedByReceiver) {
                return ErrorResults.Canceled($"The Receiver has been blocked from messaging");
            }

            var message = ChatMessage.Create(chatItem, request.ChatItemId,request.SenderId,request.Content,request.FileUrl,request.Id);
            message.MarkAsSend();
            await _unitOfWork.CreateAsync<ChatMessage>(message);

            await _unitOfWork.SaveChangeAsync();
            return SuccessResults.Ok($"The new message with id : <{request.Id}> has been sent successfully.");
        }
        catch(Exception e) {
            return ErrorResults.Canceled(e.Message);
        }

    }
}
