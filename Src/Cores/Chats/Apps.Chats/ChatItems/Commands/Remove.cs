﻿using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatItems.Commands;
public sealed record Remove(Guid ChatItemId) : IRequest<ResultStatus> {
    public static Remove New(Guid ChatItemId) => new(ChatItemId);
}

//======================= ChatItems Remove Handler
internal sealed class RemoveHandler(IChatUOW _unitOfWork) : IRequestHandler<Remove , ResultStatus> {
    public async Task<ResultStatus> Handle(Remove request , CancellationToken cancellationToken) {
        var chatItemId = request.ChatItemId;
        var chatItem = await _unitOfWork.Queries.ChatItems.FindByIdAsync(chatItemId);
        if(chatItem is null) {
            return ErrorResults.NotFound($"The {nameof(chatItemId)} : <{chatItemId}> not found.");
        }
        await _unitOfWork.DeleteAsync(chatItem);
        await _unitOfWork.SaveChangeAsync();
        return SuccessResults.Ok($"The ChatItem with ID : <{chatItemId} has been removed successfully.");
    }
}
