using Apps.Chats.ChatMessages.Commands;
using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using Command = Apps.Chats.ChatMessages.Commands;

namespace UNTests.Apps.Chats.Messages.Commands;
public class MarkMessagesAsRead {
    private readonly Mock<IChatUOW> _mocker;
    private readonly MarkMessagesAsReadHandler _handler;
    private readonly CancellationToken _cancellationToken = new();
    public MarkMessagesAsRead() {
        _mocker = new Mock<IChatUOW>();
        _handler = new(_mocker.Object);
    }

    [Fact]
    public async Task Handle_Should_Mark_Messages_AsRead() {

        //Arrange
        var request = Command.MarkMessagesAsRead.New(Guid.NewGuid());
        List<ChatMessage> messages = CreateUnreadMessages(request.ChatItemId);
        _mocker.Setup(x => x.Queries.ChatMessages.GetUnreadMessagesAsync(request.ChatItemId))
            .ReturnsAsync(messages);

        //Act
        var result = await _handler.Handle(request,_cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(SuccessResults.Ok("All Messages marked as read."));
        _mocker.Verify(x => x.Queries.ChatMessages.GetUnreadMessagesAsync(request.ChatItemId) , Times.Once);
        _mocker.Verify(x=>x.SaveChangeAsync() , Times.Once);
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
