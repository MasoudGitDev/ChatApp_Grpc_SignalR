
using Apps.Chats.ChatMessages.Commands;
using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using Command = Apps.Chats.ChatMessages.Commands;

namespace UNTests.Apps.Chats.Messages.Commands;
public class Create {

    private readonly Mock<IChatUOW> _mocker;
    private readonly CreateMessageHandler _handler;
    private readonly CancellationToken cancellationToken = new();
    public Create()
    {
        _mocker = new Mock<IChatUOW>();
        _handler = new(_mocker.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_CancelResult_When_MessageIsBlockedBy_Requester() {

        //Arrange
        var request = new Command.Create(){
            SenderId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            ChatItemId = Guid.NewGuid(),
            Content = "Message-test" ,
            Id = Guid.NewGuid(),            
        };

        var chatItem = ChatItem.Create(request.SenderId,request.ReceiverId);
        chatItem.Block(true);
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(request.SenderId , request.ReceiverId))
            .ReturnsAsync(chatItem);

        //Act
        var result = await _handler.Handle(request,cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(ErrorResults.Canceled($"You have been blocked from messaging"));
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdsAsync(request.SenderId , request.ReceiverId) , Times.Once);
        _mocker.Verify(x => x.CreateAsync(It.IsAny<ChatMessage>()) , Times.Never);
        _mocker.Verify(x => x.SaveChangeAsync() , Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_CancelResult_When_MessageIsBlockedBy_Receiver() {

        //Arrange
        var request = new Command.Create(){
            SenderId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            ChatItemId = Guid.NewGuid(),
            Content = "Message-test" ,
            Id = Guid.NewGuid(),
        };

        var chatItem = ChatItem.Create(request.SenderId,request.ReceiverId);
        chatItem.Block(false);
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(request.SenderId , request.ReceiverId))
            .ReturnsAsync(chatItem);

        //Act
        var result = await _handler.Handle(request,cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(ErrorResults.Canceled($"The Receiver has been blocked from messaging"));
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdsAsync(request.SenderId , request.ReceiverId) , Times.Once);
        _mocker.Verify(x => x.CreateAsync(It.IsAny<ChatMessage>()) , Times.Never);
        _mocker.Verify(x => x.SaveChangeAsync() , Times.Never);
    }


    [Fact]
    public async Task Handle_Should_Return_OkResult_When_EveryThingIsOk() {

        //Arrange
        var request = new Command.Create(){
            SenderId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            ChatItemId = Guid.NewGuid(),
            Content = "Message-test" ,
            Id = Guid.NewGuid(),
        };

        var chatItem = ChatItem.Create(request.SenderId,request.ReceiverId);
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(request.SenderId , request.ReceiverId))
            .ReturnsAsync(chatItem);

        _mocker.Setup(x => x.CreateAsync(It.IsAny<ChatMessage>())).Returns(Task.CompletedTask);

        //Act
        var result = await _handler.Handle(request,cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(SuccessResults.Ok($"The new message with id : <{request.Id}> has been sent successfully."));
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdsAsync(request.SenderId , request.ReceiverId) , Times.Once);
        _mocker.Verify(x => x.CreateAsync(It.IsAny<ChatMessage>()) , Times.Once);
        _mocker.Verify(x => x.SaveChangeAsync(), Times.Once);
    }


    [Fact]
    public async Task Handle_Should_Return_OkResult_When_NoBlocking_But_ChatItem_NotExist() {

        //Arrange
        var request = new Command.Create(){
            SenderId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            ChatItemId = Guid.NewGuid(),
            Content = "Message-test" ,
            Id = Guid.NewGuid(),
        };

        ChatItem? chatItem = null;
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(request.SenderId , request.ReceiverId))
            .ReturnsAsync(chatItem);
        _mocker.Setup(x => x.CreateAsync(It.IsAny<ChatItem>())).Returns(Task.CompletedTask);

        _mocker.Setup(x => x.CreateAsync(It.IsAny<ChatMessage>())).Returns(Task.CompletedTask);

        //Act
        var result = await _handler.Handle(request,cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(SuccessResults.Ok($"The new message with id : <{request.Id}> has been sent successfully."));
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdsAsync(request.SenderId , request.ReceiverId) , Times.Once);
        _mocker.Verify(x => x.CreateAsync(It.IsAny<ChatItem>()) , Times.Once);
        _mocker.Verify(x => x.CreateAsync(It.IsAny<ChatMessage>()) , Times.Once);
        _mocker.Verify(x => x.SaveChangeAsync() , Times.Once);
    }

}
