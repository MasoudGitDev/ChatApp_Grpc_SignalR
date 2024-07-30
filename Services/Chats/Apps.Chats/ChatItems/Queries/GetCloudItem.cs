using MediatR;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatItems.Queries;
public record GetCloudItem(Guid MyId) : IRequest<ResultStatus<ChatItemDto>> {
    public static GetCloudItem New(Guid myId) => new(myId);
}

//======================= handler
internal sealed class GetCloudItemHandler(IChatUOW _unitOfWork) 
    : IRequestHandler<GetCloudItem , ResultStatus<ChatItemDto>> {
    public async Task<ResultStatus<ChatItemDto>> Handle(GetCloudItem request , CancellationToken cancellationToken) {
        try {
            var findUser = await _unitOfWork.Queries.Users.FindByIdAsync(request.MyId);
            if(findUser is null) {
                return ErrorResults.Canceled<ChatItemDto>($"The user with ID : <{request.MyId}> has not been found.");
            }
            ChatItemDto result = new(){
                DisplayName = "فضای شخصی" ,
                ReceiverId = findUser.Id ,
                LogoUrl = findUser.ImageUrl ,
                UnReadMessages = 0 ,
                Id = Guid.NewGuid(),
            };
            var cloudItem = await _unitOfWork.Queries.ChatItems.GetByIdsAsync(request.MyId , request.MyId);

            if(cloudItem is not null) {
                result.Id = cloudItem.Id;
            }
            return SuccessResults.Ok(result);
        }
        catch(Exception ex) {
            return ErrorResults.Canceled<ChatItemDto>(ex.Message);
        }
    }
}
