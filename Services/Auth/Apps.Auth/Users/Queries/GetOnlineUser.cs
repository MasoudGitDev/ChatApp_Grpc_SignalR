using Domains.Auth.Online.Aggregate;
using Domains.Auth.Online.Queries;
using MediatR;
using Shared.Server.Models.Results;

namespace Apps.Auth.Users.Queries;
public sealed record GetOnlineUser(Guid UserId) : IRequest<ResultStatus<OnlineUser>>;

internal sealed class GetOnlineUserHandler(IOnlineUserQueries _queries) :
    IRequestHandler<GetOnlineUser , ResultStatus<OnlineUser>> {
    public async Task<ResultStatus<OnlineUser>> Handle(GetOnlineUser request , CancellationToken cancellationToken) {
        var findOnlineUser = await _queries.GetByIdAsync(request.UserId);
        if(findOnlineUser is null) {
            return ErrorResults.NotFound($"There is no any record with id:{request.UserId} in OnlineUsers table." ,
                findOnlineUser);
        }
        return SuccessResults.Ok(findOnlineUser);
    }
}
