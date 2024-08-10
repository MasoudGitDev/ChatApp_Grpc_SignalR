using Mapster;
using MediatR;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatItems.Queries;
/// <summary>
/// OtherId can be RequesterId Or ReceiverId.
/// </summary>
public sealed record FindChatItem(Guid MyId , Guid OtherId):IRequest<ResultStatus<ChatItemDto>> {
    public static FindChatItem New(Guid myId, Guid OtherId) => new(myId, OtherId);
}


//========================== handler
internal sealed class FindChatItemHandler(IChatUOW _unitOfWork) : IRequestHandler<FindChatItem , ResultStatus<ChatItemDto>> {
    public async Task<ResultStatus<ChatItemDto>> Handle(FindChatItem request , CancellationToken cancellationToken) {
        var chatItemDTO = (await _unitOfWork.Queries.ChatItems.FindByIdsAsync(request.MyId,request.OtherId))
            .Adapt<ChatItemDto>();
        if(chatItemDTO is null) {
            chatItemDTO = new() {
                DisplayName = "Test" ,
                Id = Guid.NewGuid(),
                ReceiverId = request.OtherId ,                
                LogoUrl = "img-test" ,
                UnReadMessages = 0
            };
        }
        return SuccessResults.Ok(chatItemDTO);
    }
}
