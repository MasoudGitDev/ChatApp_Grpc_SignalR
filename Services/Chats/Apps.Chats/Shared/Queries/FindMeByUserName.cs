using Domains.Auth.User.Aggregate;
using MediatR;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.Shared.Queries;

public sealed record FindMeByUserName(string UserName) : IRequest<AppUser> {
    public static FindMeByUserName New(string userName) => new(userName);
}

//=================== Handler
internal sealed class FindMeByUserNameHandler(IChatUOW _unitOfWork) : IRequestHandler<FindMeByUserName , AppUser> {
    public async Task<AppUser> Handle(FindMeByUserName request , CancellationToken cancellationToken) {
        return await _unitOfWork.Queries.Users.FindByUserNameAsync(request.UserName) ?? AppUser.InvalidUser;
    }
}

