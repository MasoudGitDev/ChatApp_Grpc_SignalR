using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatRequests.Commands;

// ChatRequests Block Model
public sealed record Block(Guid ChatRequestId , Guid MyId) : IRequest<ResultStatus> {
    public static Block New(Guid ChatRequestId , Guid MyId) => new(ChatRequestId , MyId);
}

// ChatRequests Block Handler
internal sealed class BlockHandler(IChatUOW _unitOfWork)
    : ChatRequestHandler<Block , ResultStatus>(_unitOfWork) {
    public override async Task<ResultStatus> Handle(Block request , CancellationToken cancellationToken)
        => await DoAsync(request.ChatRequestId , async (model) => await model.BlockAsync() , okMessage);

    private const string okMessage = "The request has been blocked successfully.";
}
