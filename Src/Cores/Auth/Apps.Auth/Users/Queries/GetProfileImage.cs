using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Auth.Users.Queries;
public sealed record GetProfileImage(Guid UserId , string FileDirectory) : IRequest<ResultStatus<string>> {
    public static GetProfileImage New(Guid userId , string fileDirectory) => new(userId , fileDirectory);
}

//============= handler
internal sealed class GetProfileImageHandler(IChatUOW _unitOfWork) : IRequestHandler<GetProfileImage , ResultStatus<string>> {
    public async Task<ResultStatus<string>> Handle(GetProfileImage request , CancellationToken cancellationToken) {
        var findUser = await _unitOfWork.Queries.Users.FindByIdAsync(request.UserId);
        if(findUser is null) {
            return ErrorResults.Canceled<string>($"The userId :<{request.UserId}> is not valid.");
        }
        string fileName = findUser.ImageUrl ;
        if(String.IsNullOrWhiteSpace(fileName)) {
            return ErrorResults.Canceled<string>($"The user has not any image.");
        }
        return SuccessResults.Ok<string>(Path.Combine(request.FileDirectory , fileName));
    }
}
