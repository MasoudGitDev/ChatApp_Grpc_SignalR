using Domains.Chats.Requests.Aggregate;
using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatRequests;
internal abstract class ChatRequestHandler<T, R>(IChatUOW _unitOfWork)
    : IRequestHandler<T , R> where T : IRequest<R> where R : IResultStatus {
    public abstract Task<R> Handle(T request , CancellationToken cancellationToken);

    protected async Task<ResultStatus> DoAsync(Guid chatRequestId , Func<ChatRequest , Task> actions , string resultMessage) {
        var model = await _unitOfWork.Queries.ChatRequests.FindByIdAsync(chatRequestId);
        if(model is null) {
            return ErrorResults.NotFound($"There is no any ChatRequest record with id :<{chatRequestId}>.");
        }
        await actions.Invoke(model);
        await _unitOfWork.SaveChangeAsync();
        return SuccessResults.Ok(resultMessage);
    }
}
