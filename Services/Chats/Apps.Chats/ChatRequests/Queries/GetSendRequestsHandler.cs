using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using UnitOfWorks.Extensions;

namespace Apps.Chats.ChatRequests.Queries;

public record GetSendRequests(Guid MyId) : IRequest<ResultStatus<List<ChatRequestItem>>> {
    public static GetSendRequests New(Guid MyId) => new(MyId);
}

//============ Handler
internal sealed class GetSendRequestsHandler(IChatUOW _unitOfWork)
    : ChatRequestHandler<GetSendRequests , ResultStatus<List<ChatRequestItem>>>(_unitOfWork.HasValue()) {
    public override async Task<ResultStatus<List<ChatRequestItem>>> Handle(GetSendRequests request ,
        CancellationToken cancellationToken) {
        return new(true , [] , await _unitOfWork.Queries.ChatRequests.GetSendRequestsAsync(request.MyId));
    }
}
