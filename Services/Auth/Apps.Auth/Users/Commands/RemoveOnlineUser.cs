using Domains.Auth.Online.Aggregate;
using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Auth.Users.Commands;
public sealed record RemoveOnlineUser(Guid MyId) : IRequest<ResultStatus> {
    public static RemoveOnlineUser New(Guid myId) => new(myId);
}



//======================= Handler
internal sealed class RemoveOnlineUserHandler(IChatUOW _unitOfWork) : IRequestHandler<RemoveOnlineUser , ResultStatus> {
    public async Task<ResultStatus> Handle(RemoveOnlineUser request , CancellationToken cancellationToken) {
        string message = "You has not been in OnlineUsers!";
        var onlineUser = await _unitOfWork.Queries.OnlineUsers.GetByIdAsync(request.MyId);
        if(onlineUser is not null) {
            message = "You has been removed from OnlineUsers successfully.";
            await _unitOfWork.DeleteAsync(onlineUser);
            await _unitOfWork.SaveChangeAsync();
        }       
        return SuccessResults.Ok(message);
    }
}
