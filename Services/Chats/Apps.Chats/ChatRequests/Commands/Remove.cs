using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using UnitOfWorks.Extensions;

namespace Apps.Chats.ChatRequests.Commands;

// ChatRequests Block Model
public sealed record Remove(Guid ChatRequestId) : IRequest<ResultStatus> {
    public static Remove New(Guid ChatRequestId) => new(ChatRequestId);
}

// ChatRequests Remove Handler
internal sealed class RemoveHandler(IChatUOW _unitOfWork)
    : ChatRequestHandler<Remove , ResultStatus>(_unitOfWork.HasValue()) {
    public override async Task<ResultStatus> Handle(Remove request , CancellationToken cancellationToken)
        => await DoAsync(request.ChatRequestId , async (model) => await _unitOfWork.DeleteAsync(model) , okMessage);

    private const string okMessage =  "The request has been removed successfully.";
}

