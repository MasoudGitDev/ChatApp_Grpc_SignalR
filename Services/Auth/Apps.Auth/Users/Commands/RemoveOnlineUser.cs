using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Auth.Users.Commands;
public sealed record RemoveOnlineUser(Guid MyId) : IRequest<ResultStatus<Guid>> {
    public static RemoveOnlineUser New(Guid myId) => new(myId);
}



//======================= Handler
internal sealed class RemoveOnlineUserHandler(IChatUOW _unitOfWork) : IRequestHandler<RemoveOnlineUser , ResultStatus<Guid>> {
    public async Task<ResultStatus<Guid>> Handle(RemoveOnlineUser request , CancellationToken cancellationToken) {
        string message = "You have not been in OnlineUsers!";
        var onlineUser = await _unitOfWork.Queries.OnlineUsers.GetByIdAsync(request.MyId);
        if(onlineUser is not null) {
            message = "You have been removed from OnlineUsers successfully.";
            await _unitOfWork.DeleteAsync(onlineUser);
            await _unitOfWork.SaveChangeAsync();
        }
        return SuccessResults.Ok(message , request.MyId);
    }
}
