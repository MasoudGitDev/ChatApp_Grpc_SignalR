using MediatR;
using Shared.Server.Dtos.User;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using UnitOfWorks.Extensions;

namespace Apps.Auth.Users.Queries;
public sealed record GetOnlineUsers : IRequest<ResultStatus<List<UserBasicInfoDto>>> {
    public static GetOnlineUsers New() => new();
}

internal sealed class GetOnlineUsersHandler(IChatUOW _unitOfWork) :
    IRequestHandler<GetOnlineUsers , ResultStatus<List<UserBasicInfoDto>>> {
    public async Task<ResultStatus<List<UserBasicInfoDto>>> Handle(GetOnlineUsers request ,
        CancellationToken cancellationToken) {
        return SuccessResults.Ok(await GetUsersWithBasicInfoAsync(await GetOnlineUserIdsAsync()));
    }

    private async Task<List<Guid>> GetOnlineUserIdsAsync() => ( await _unitOfWork.Queries.OnlineUsers.GetIdsAsync() );
    private async Task<List<UserBasicInfoDto>> GetUsersWithBasicInfoAsync(List<Guid> onlineUserIds) {
        List<UserBasicInfoDto> onlineUsersWithBasicInfo = [];
        foreach(var onlineUserId in onlineUserIds) {
            var (flag, user) = await GetUserWithBasicInfoAsync(onlineUserId);
            if(flag) {
                onlineUsersWithBasicInfo.Add(user!);
            }
        }
        return onlineUsersWithBasicInfo;
    }
    private async Task<(bool Flag, UserBasicInfoDto? BasicInfo)> GetUserWithBasicInfoAsync(Guid userId) {
        var user = (await _unitOfWork.Queries.Users.FindByIdAsync(userId))?.ToBasicInfo();
        if(user is null) {
            return (false, user);
        }
        return (false, user);
    }

}
