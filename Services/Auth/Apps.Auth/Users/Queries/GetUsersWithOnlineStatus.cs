using MediatR;
using Shared.Server.Dtos.User;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using UnitOfWorks.Extensions;

namespace Apps.Auth.Users.Queries;
public sealed record GetUsersWithOnlineStatus(int PageNumber,int Size) : IRequest<ResultStatus<List<OnlineUserDto>>> {
    public static GetUsersWithOnlineStatus New(int pageNumber = 1,int size = 20) => new(pageNumber,size);
}

internal sealed class GetUsersWithOnlineStatusHandler(IChatUOW _unitOfWork)
    : IRequestHandler<GetUsersWithOnlineStatus , ResultStatus<List<OnlineUserDto>>> {
    public async Task<ResultStatus<List<OnlineUserDto>>> Handle(GetUsersWithOnlineStatus request ,
        CancellationToken cancellationToken) {
        List<OnlineUserDto> newUsersWithOnlineStatus = [];
        try {           
            var users = await _unitOfWork.Queries.Users.GetUsersAsync(request.PageNumber,request.Size);
            foreach(var user in users) {
                var item =  (await _unitOfWork.Queries.OnlineUsers.GetInfoByIdAsync(user.Id));
                bool isOnline = (await _unitOfWork.Queries.OnlineUsers.GetInfoByIdAsync(user.Id)) != null;
                newUsersWithOnlineStatus.Add(OnlineUserDto.New(user.ProfileId,user.DisplayName,user.ImageUrl, isOnline));
            }
            return SuccessResults.Ok(newUsersWithOnlineStatus);
        }
        catch(Exception ex) { 
            return ErrorResults.NotFound(ex.Message, newUsersWithOnlineStatus);
        }
    }
}
