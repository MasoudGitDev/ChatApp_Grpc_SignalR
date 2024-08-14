using Domains.Auth.User.Aggregate;
using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using QueryModel = Apps.Chats.ChatItems.Queries;

namespace UNTests.Apps.Chats.ChatItems.Queries;
public class GetChatItems {

    private readonly Mock<IChatUOW> _mocker;
    private readonly QueryModel.GetChatItemsHandler _handler;
    private readonly CancellationToken CancellationToken = new();

    public GetChatItems() {
        _mocker = new Mock<IChatUOW>();
        _handler = new(_mocker.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_OkResult_With_ExpectedItems() {

        //Arrange
        Guid myId = Guid.NewGuid();
        var request = QueryModel.GetChatItems.New(myId, 1 ,30);

        List<ChatItem> items = CreateFakeItems(request.MyId,Guid.NewGuid());
        _mocker.Setup(x => x.Queries.ChatItems
            .GetItemsByUserIdAsync(request.MyId , request.PageNumber , request.PageSize))
            .ReturnsAsync(items);

        var mockReceiver = AppUser.Create(new(){
            DisplayName = "test-display-name" ,
            Email = "test-email" ,
            Password = "test -password" ,
            UserName = "test-username" ,
            Id = Guid.NewGuid(),
        });
        _mocker.Setup(x => x.Queries.Users.FindByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockReceiver);

        var unReadMessages = new List<ChatMessage>();
        _mocker.Setup(x => x.Queries.ChatMessages.GetAllAsync(It.IsAny<Guid>() ,
            false ,
            request.PageNumber ,
            request.PageSize
        )).ReturnsAsync(unReadMessages);

        List<ChatItemDto> expectedItems = items
            .Where(x=> x.RequesterId == myId || x.ReceiverId == myId)
            .Select((x,i)=> new ChatItemDto(){
                Id = x.Id,
                DisplayName = "test-display-name" ,
                ReceiverId = mockReceiver.Id,
                UnReadMessages = 0,
            }).ToList();

        //Act
        var result = await _handler.Handle(request,CancellationToken);

        //Assert
        result.Should().BeEquivalentTo(SuccessResults.Ok(expectedItems));
        _mocker.Verify(x => x.Queries.ChatItems
             .GetItemsByUserIdAsync(request.MyId , request.PageNumber , request.PageSize) , Times.Once);
        _mocker.Verify(x => x.Queries.Users.FindByIdAsync(It.IsAny<Guid>()) , Times.AtLeastOnce);
        _mocker.Verify(x => x.Queries.ChatMessages.GetAllAsync(It.IsAny<Guid>() ,
            false ,
            request.PageNumber ,
            request.PageSize) , Times.AtLeastOnce);

    }

    //============================
    private List<ChatItem> CreateFakeItems(Guid requesterId , Guid receiverId) {
        List<ChatItem> items = [];
        for(int i = 1 ; i <= 10 ; i++) {
            items.Add(ChatItem.Create(requesterId , receiverId));
        }
        for(int i = 1 ; i <= 10 ; i++) {
            items.Add(ChatItem.Create(receiverId , requesterId));
        }
        return items;
    }

}
