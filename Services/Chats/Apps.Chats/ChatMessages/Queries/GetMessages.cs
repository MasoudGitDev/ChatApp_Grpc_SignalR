using Mapster;
using MediatR;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatMessages.Queries;
public sealed record GetMessages(Guid ChatItemId) : IRequest<ResultStatus<List<GetMessageDto>>> {
    public static GetMessages New(Guid chatItemId) => new(chatItemId);
}


//=================== handler
internal sealed class GetMessagesHandler(IChatUOW _unitOfWork) : IRequestHandler<GetMessages , ResultStatus<List<GetMessageDto>>> {
    public async Task<ResultStatus<List<GetMessageDto>>> Handle(GetMessages request , CancellationToken cancellationToken) {
        try {
            var messages = await _unitOfWork.Queries.ChatMessages.GetAllAsync(request.ChatItemId);
            return SuccessResults.Ok(messages.Adapt<List<GetMessageDto>>());
        }
        catch(Exception e) {
            return ErrorResults.Canceled<List<GetMessageDto>>(e.Message);
        }
    }
}
