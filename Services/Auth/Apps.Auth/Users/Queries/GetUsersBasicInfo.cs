using Domains.Auth.Queries;
using Domains.Auth.User.ValueObjects;
using Mapster;
using MediatR;
using Shared.Server.Dtos.User;
using Shared.Server.Models.Results;

namespace Apps.Auth.Users.Queries;
/// <summary>
/// HomeUsers means The Users that have simple info to show in home page
/// </summary>
public sealed record GetUsersBasicInfo() : IRequest<ResultStatus<List<UserBasicInfoDto>>> {
    public static GetUsersBasicInfo New() => new();
}


//============= Query Handler
internal sealed class GetHomeUsersHandler(IUserQueries _userQueries)
    : IRequestHandler<GetUsersBasicInfo , ResultStatus<List<UserBasicInfoDto>>> {
    public async Task<ResultStatus<List<UserBasicInfoDto>>> Handle(GetUsersBasicInfo request , CancellationToken cancellationToken) {
        var homeUsers = await _userQueries.GetUsersAsync();
        TypeAdapterConfig<ProfileId , string>.NewConfig().MapWith(x => x.Value);
        return SuccessResults.Ok(homeUsers.Adapt<List<UserBasicInfoDto>>());
    }
}
