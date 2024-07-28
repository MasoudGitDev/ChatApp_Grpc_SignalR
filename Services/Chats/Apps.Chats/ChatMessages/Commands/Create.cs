using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.Aggregate;
using Mapster;
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
            var item = await _unitOfWork.Queries.ChatItems.FindByIdAsync(request.ChatItemId);
            // create chatItem if not exist
            if(item is null) {
                item = ChatItem.Create(request.SenderId , request.ReceiverId);
                await _unitOfWork.CreateAsync(item);
            }
            
            if(item.IsBlockedByRequester) {
                return ErrorResults.Canceled($"You have been blocked from messaging");
            }
            if(item.IsBlockedByReceiver) {
                return ErrorResults.Canceled($"The Receiver has been blocked from messaging");
            }

            var message = ChatMessage.Create(request.ChatItemId,request.SenderId,request.Content,request.FileUrl,request.Id);
            await _unitOfWork.CreateAsync(message);

            await _unitOfWork.SaveChangeAsync();
            return SuccessResults.Ok($"The new message with id : <{request.Id}> has been sent successfully.");
        }
        catch(Exception e) {
            return ErrorResults.Canceled(e.Message);
        }

    }
}
