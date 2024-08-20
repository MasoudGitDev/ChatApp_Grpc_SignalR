using Domains.Auth.User.ValueObjects;
using MediatR;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Auth.Users.Commands;
public sealed record UpdateProfileLogo(Guid UserId , ImageUrl ImageUrl) : IRequest<ResultStatus> {
    public static UpdateProfileLogo New(Guid userId , ImageUrl imageUrl) => new(userId , imageUrl);
}

//----------- handler
internal sealed class UploadProfileLogoHandler(IChatUOW _unitOfWork) : IRequestHandler<UpdateProfileLogo , ResultStatus> {
    public async Task<ResultStatus> Handle(UpdateProfileLogo request , CancellationToken cancellationToken) {
        var findUser = await _unitOfWork.Queries.Users.FindByIdAsync(request.UserId);
        if(findUser is null) {
            return ErrorResults.NotFound($"The userId : <{request.UserId}> not found.");
        }
        findUser.Update(request.ImageUrl);
        await _unitOfWork.SaveChangeAsync();
        return SuccessResults.Ok("The new image has been updated successfully.");
    }
}
