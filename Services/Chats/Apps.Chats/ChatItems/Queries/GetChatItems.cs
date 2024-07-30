using Domains.Chats.Item.Aggregate;
using MediatR;
using Microsoft.VisualBasic;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace Apps.Chats.ChatItems.Queries;
public record GetChatItems(Guid MyId , int PageNumber = 1 , int PageSize = 20) : IRequest<ResultStatus<List<ChatItemDto>>> {
    public static GetChatItems New(Guid myId , int pageNumber = 1 , int pageSize = 20) => new(myId , pageNumber , pageSize);
}

//=============== handler
internal sealed class GetChatItemsHandler(IChatUOW _unitOfWork) : IRequestHandler<GetChatItems , ResultStatus<List<ChatItemDto>>> {
    public async Task<ResultStatus<List<ChatItemDto>>> Handle(GetChatItems request , CancellationToken cancellationToken) {
        try {
            var chatItems = await _unitOfWork.Queries.ChatItems.GetByIdAsync(request.MyId,request.PageNumber,request.PageSize);
            var itemDTOs = new List<ChatItemDto>();
            foreach(var chatItem in chatItems) {
                // Avoid Displaying Cloud Item
                if(chatItem.ReceiverId == request.MyId && chatItem.RequesterId == request.MyId) { 
                    continue;
                }
                var findReceiver = await _unitOfWork.Queries.Users.FindByIdAsync(GetReceiverId(request.MyId,chatItem));
                if(findReceiver is null) {
                    continue;
                }
                itemDTOs.Add(new() {
                    Id = chatItem.Id ,
                    DisplayName = findReceiver.DisplayName ,
                    LogoUrl = findReceiver.ImageUrl ,
                    ReceiverId = chatItem.ReceiverId ,
                    UnReadMessages = 0
                });
            }
            return SuccessResults.Ok(itemDTOs);
        }
        catch(Exception ex) { 
            return ErrorResults.Canceled<List<ChatItemDto>>(ex.Message);
        }
    }

    private static Guid GetReceiverId(Guid myId , ChatItem item) {
        return myId == item.RequesterId ? item.ReceiverId : item.RequesterId;
    }
}
