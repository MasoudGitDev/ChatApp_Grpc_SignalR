using Apps.Chats.ChatItems.Commands.Model;
using Domains.Chats.Shared;
using MediatR;
using Shared.Server.Models.Results;

namespace Apps.Chats.ChatItems.Commands.Handlers;
internal sealed class RemoveHandler(IChatUOW _unitOfWork) : IRequestHandler<RemoveRequest , ResultStatus> {
    public async Task<ResultStatus> Handle(RemoveRequest request , CancellationToken cancellationToken) {
        var chatItemId = request.ChatItemId;
        var chatItem = await _unitOfWork.Queries.ChatItems.FindByIdAsync(chatItemId);
        if(chatItem is null) {
            return ChatItemResult.NotFound($"The {nameof(chatItemId)} : <{chatItemId}> not found.");
        }
        await _unitOfWork.DeleteAsync(chatItem);
        await _unitOfWork.SaveChangeAsync();
        return ChatItemResult.Removed(chatItemId);
    }
}
