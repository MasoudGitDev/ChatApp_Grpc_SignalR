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
        var chatItem = (await _unitOfWork.Queries.ChatItems.FindByIdsAsync(request.MyId,request.OtherId));
        if(chatItem is null) {
            return SuccessResults.Ok(CreateItemDto(request.OtherId));
        }
        return SuccessResults.Ok(chatItem.Adapt<ChatItemDto>());
    }

    private ChatItemDto CreateItemDto(Guid otherId) {
        ChatItemDto chatItemDTO = new() {
            DisplayName = "Test" ,
            Id = Guid.NewGuid(),
            ReceiverId = otherId ,
            LogoUrl = "img-test" ,
            UnReadMessages = 0
        };
        return chatItemDTO;
    }
}
