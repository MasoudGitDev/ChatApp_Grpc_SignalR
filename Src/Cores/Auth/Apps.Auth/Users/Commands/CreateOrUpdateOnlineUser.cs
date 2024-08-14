using Domains.Auth.Online.Aggregate;
using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Auth.Users.Commands;
public sealed record CreateOrUpdateOnlineUser(Guid MyId) : IRequest<ResultStatus<Guid>> {
    public static CreateOrUpdateOnlineUser New(Guid myId) => new(myId);
}

internal sealed class CreateOrUpdateOnlineUserHandler(IChatUOW _unitOfWork) : IRequestHandler<CreateOrUpdateOnlineUser , ResultStatus<Guid>> {
    public async Task<ResultStatus<Guid>> Handle(CreateOrUpdateOnlineUser request , CancellationToken cancellationToken) {
        string message = "You have been added to OnlineUsers successfully.";
        var onlineUser = await _unitOfWork.Queries.OnlineUsers.GetByIdAsync(request.MyId);
        if(onlineUser is null) {
            await _unitOfWork.CreateAsync(OnlineUser.Create(request.MyId));
        }
        else {
            message = "Your Online Status has been changed successfully";
            await onlineUser.UpdateAsync(DateTime.UtcNow);
        }
        await _unitOfWork.SaveChangeAsync();
        return SuccessResults.Ok(message , request.MyId);
    }
}
