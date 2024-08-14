using Mapster;
using MediatR;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatMessages.Queries;
public sealed record GetMessages(Guid ChatItemId , int PageNumber = 1 , int PageSize = 50) : IRequest<ResultStatus<List<GetMessageDto>>> {
    public static GetMessages New(Guid chatItemId , int pageNumber = 1 , int pageSize = 50)
        => new(chatItemId , pageNumber , pageSize);
}


//=================== handler
internal sealed class GetMessagesHandler(IChatUOW _unitOfWork) : IRequestHandler<GetMessages , ResultStatus<List<GetMessageDto>>> {
    public async Task<ResultStatus<List<GetMessageDto>>> Handle(GetMessages request , CancellationToken cancellationToken) {
        try {
            var messages = await _unitOfWork.Queries.ChatMessages.GetAllAsync(request.ChatItemId,true,
                request.PageNumber,
                request.PageSize);
            return SuccessResults.Ok(messages.Adapt<List<GetMessageDto>>());
        }
        catch(Exception e) {
            return ErrorResults.Canceled<List<GetMessageDto>>(e.Message);
        }
    }
}
