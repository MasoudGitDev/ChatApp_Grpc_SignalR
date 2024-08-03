using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatMessages.Commands;
public sealed record MarkMessagesAsRead(Guid ChatItemId) : IRequest<ResultStatus> {
    public static MarkMessagesAsRead New(Guid chatItemId) => new(chatItemId);
}

//=========================== handler
internal sealed class MarkMessagesAsReadHandler(IChatUOW _unitOfWork) : IRequestHandler<MarkMessagesAsRead , ResultStatus> {
    public async Task<ResultStatus> Handle(MarkMessagesAsRead request , CancellationToken cancellationToken) {
        var unreadMessages = (await _unitOfWork.Queries.ChatMessages.GetUnreadMessagesAsync(request.ChatItemId));
        foreach (var unreadMessage in unreadMessages) {
            unreadMessage.MarkAsRead();
        }
        await _unitOfWork.SaveChangeAsync();
        return SuccessResults.Ok("All Messages marked as read.");
    }
}
