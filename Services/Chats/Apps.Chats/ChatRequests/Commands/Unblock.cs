using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatRequests.Commands;

// ChatRequests Unblock Model
public sealed record Unblock(Guid ChatRequestId , Guid MyId) : IRequest<ResultStatus> {
    public static Unblock New(Guid ChatRequestId , Guid MyId) => new(ChatRequestId , MyId);
}

// ChatRequests Unblock Handler
internal sealed class UnblockHandler(IChatUOW _unitOfWork)
    : ChatRequestHandler<Unblock , ResultStatus>(_unitOfWork) {
    public override async Task<ResultStatus> Handle(Unblock request , CancellationToken cancellationToken)
        => await DoAsync(request.ChatRequestId , async (model) => await model.UnBlockAsync() , okMessage);

    private const string okMessage = "The request has been unblocked successfully.";
}

