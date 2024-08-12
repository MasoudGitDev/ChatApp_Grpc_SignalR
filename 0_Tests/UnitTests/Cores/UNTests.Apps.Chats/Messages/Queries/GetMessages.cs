using Apps.Chats.ChatMessages.Queries;
using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.Aggregate;
using FluentAssertions;
using Mapster;
using Moq;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using Query = Apps.Chats.ChatMessages.Queries;

namespace UNTests.Apps.Chats.Messages.Queries;
public class GetMessages {

    private readonly Mock<IChatUOW> _mocker;
    private readonly GetMessagesHandler _handler;
    private readonly CancellationToken cancellationToken = new();
    public GetMessages() {
        _mocker = new Mock<IChatUOW>();
        _handler = new(_mocker.Object);
    }

    [Fact]
    public async Task Handle_Should_Convert_Messages_To_GetMessagesDto() {

        //Arrange
        var request = Query.GetMessages.New(Guid.NewGuid(),1,50);
        List<ChatMessage> messages =CreateUnreadMessages(request.ChatItemId);
        _mocker.Setup(x => x.Queries.ChatMessages.GetAllAsync(request.ChatItemId , true ,
            request.PageNumber ,
            request.PageSize
        )).ReturnsAsync(messages);

        var expectedMessage = messages.Adapt<List<GetMessageDto>>();

        //Act
        var result = await _handler.Handle(request, cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(SuccessResults.Ok(expectedMessage));
        _mocker.Verify(x => x.Queries.ChatMessages.GetAllAsync(request.ChatItemId , true ,
            request.PageNumber ,
            request.PageSize
        ) , Times.Once);
    }



    //============== privates
    private List<ChatMessage> CreateUnreadMessages(Guid itemId) {
        List<ChatMessage> messages = [];
        ChatItem item = ChatItem.Create(itemId,Guid.NewGuid(),Guid.NewGuid());
        for(var i = 1 ; i <= 10 ; i++) {
            messages.Add(ChatMessage.Create(item , item.Id , item.RequesterId , "test" + i));
        }
        return messages;
    }
}
