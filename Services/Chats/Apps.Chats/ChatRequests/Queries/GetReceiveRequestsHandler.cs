using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using UnitOfWorks.Extensions;

namespace Apps.Chats.ChatRequests.Queries;


/// ChatRequests.Queries GetReceiveRequests Model
public sealed record GetReceiveRequests(Guid MyId) : IRequest<ResultStatus<List<ChatRequestItem>>> {
    public static GetReceiveRequests New(Guid MyId) => new(MyId);
}

/// ChatRequests.Queries GetReceiveRequests Handler
internal sealed class GetReceiveRequestsHandler(IChatUOW _unitOfWork)
    : ChatRequestHandler<GetReceiveRequests , ResultStatus<List<ChatRequestItem>>>(_unitOfWork.HasValue()) {
    public override async Task<ResultStatus<List<ChatRequestItem>>> Handle(GetReceiveRequests request ,
        CancellationToken cancellationToken) {
        return new(true , [] , await _unitOfWork.Queries.ChatRequests.GetReceiveRequestsAsync(request.MyId));
    }
}
