using Apps.Chats.Shared.Models;
using Domains.Auth.User.Aggregate;
using MediatR;

namespace Apps.Chats.Shared.Handlers;
internal sealed class FindMeByUserNameHandler(IChatUOW _unitOfWork) : IRequestHandler<FindMeByUserNameRequest , AppUser> {
    public async Task<AppUser> Handle(FindMeByUserNameRequest request , CancellationToken cancellationToken) {
        return await _unitOfWork.Queries.Users.FindByUserNameAsync(request.UserName) ?? AppUser.Empty;
    }
}
