using Domains.Auth.Online.Aggregate;
using Domains.Auth.Online.Queries;
using MediatR;
using Shared.Server.Models.Results;

namespace Apps.Auth.Users.Queries;
public sealed record GetOnlineUsers(Guid UserId) : IRequest<ResultStatus<List<OnlineUser>>>;

internal sealed class GetOnlineUsersHandler(IOnlineUserQueries _queries) :
    IRequestHandler<GetOnlineUsers , ResultStatus<List<OnlineUser>>> {
    public async Task<ResultStatus<List<OnlineUser>>> Handle(GetOnlineUsers request , CancellationToken cancellationToken) {
        var findOnlineUsers = await _queries.GetAllAsync();
        if(findOnlineUsers is null) {
            return ErrorResults.NotFound($"There is no any online users." , findOnlineUsers);
        }
        return SuccessResults.Ok(findOnlineUsers);
    }
}
