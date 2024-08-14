using Domains.Auth.Online.Aggregate;
using Domains.Auth.Online.Queries;
using MediatR;
using Shared.Server.Models.Results;

namespace Apps.Auth.Users.Queries;
public sealed record GetOnlineUser(Guid UserId) : IRequest<ResultStatus<OnlineUser>> {
    public static GetOnlineUser New(Guid userId) => new(userId);
}

internal sealed class GetOnlineUserHandler(IOnlineUserQueries _queries) :
    IRequestHandler<GetOnlineUser , ResultStatus<OnlineUser>> {
    public async Task<ResultStatus<OnlineUser>> Handle(GetOnlineUser request , CancellationToken cancellationToken) {
        var findOnlineUser = await _queries.GetByIdAsync(request.UserId);
        if(findOnlineUser is null) {
            return ErrorResults.NotFound($"Not Found any online user with id:{request.UserId}" , findOnlineUser);
        }
        return SuccessResults.Ok(findOnlineUser);
    }
}
