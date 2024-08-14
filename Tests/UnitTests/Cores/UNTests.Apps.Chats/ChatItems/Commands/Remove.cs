using Moq.AutoMock;
using Moq;
using UnitOfWorks.Abstractions;
using Command = Apps.Chats.ChatItems.Commands;
using Domains.Chats.Item.Aggregate;
using FluentAssertions;
using Shared.Server.Models.Results;

namespace UNTests.Apps.Chats.ChatItems.Commands;
public class Remove
{

    private readonly Mock<IChatUOW> _mocker;
    private readonly Command.RemoveHandler handler;

    private readonly CancellationToken cancellationToken = new();

    public Remove()
    {
        _mocker = new Mock<IChatUOW>();
        handler = new(_mocker.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_NotFound_If_ChatItem_NotExist()
    {
        //Arrange
        var removeModel = Command.Remove.New(Guid.NewGuid());
        ChatItem? chatItem = null;
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(chatItem);

        //Act
        var result = await handler.Handle(removeModel, cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(ErrorResults.NotFound($"The chatItemId : <{removeModel.ChatItemId}> not found."));
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_ChatItem_Successfully()
    {
        //Arrange
        var removeModel = Command.Remove.New(Guid.NewGuid());
        ChatItem chatItem = ChatItem.Create(Guid.NewGuid(), Guid.NewGuid());
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(chatItem);

        //Act
        var result = await handler.Handle(removeModel, cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(
            SuccessResults.Ok($"The ChatItem with ID : <{removeModel.ChatItemId} has been removed successfully."));
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mocker.Verify(x => x.DeleteAsync(chatItem), Times.Once);
        _mocker.Verify(x => x.SaveChangeAsync(), Times.Once);
    }
}
