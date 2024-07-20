using Domains.Auth.Queries;
using Mapster;
using MediatR;
using Shared.Server.Dtos.User;
using Shared.Server.Models.Results;

namespace Apps.Auth.Users.Queries;
/// <summary>
/// HomeUsers means The Users that have simple info to show in home page
/// </summary>
public record GetHomeUsers() : IRequest<ResultStatus<List<UserBasicInfoDto>>> {
    public static GetHomeUsers New() => new();
}


//============= Query Handler
internal sealed class GetHomeUsersHandler(IUserQueries _userQueries)
    : IRequestHandler<GetHomeUsers , ResultStatus<List<UserBasicInfoDto>>> {
    public async Task<ResultStatus<List<UserBasicInfoDto>>> Handle(GetHomeUsers request , CancellationToken cancellationToken) {
        var homeUsers = await _userQueries.GetUsersAsync();
        return SuccessResults.Ok(homeUsers.Adapt<List<UserBasicInfoDto>>());
    }
}
