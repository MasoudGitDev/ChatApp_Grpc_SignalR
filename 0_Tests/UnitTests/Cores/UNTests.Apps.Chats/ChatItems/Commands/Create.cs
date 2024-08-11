using Domains.Auth.User.Aggregate;
using Domains.Chats.Item.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using Command = Apps.Chats.ChatItems.Commands;


namespace UNTests.Apps.Chats.ChatItems.Commands;
public class Create
{
    private readonly Mock<IChatUOW> _mocker;
    private readonly Command.CreateHandler handler;

    private readonly CancellationToken cancellationToken = new();

    public Create()
    {
        _mocker = new Mock<IChatUOW>();
        handler = new(_mocker.Object);
    }


    [Fact]
    public async Task Handle_Should_Return_CanceledResult_For_SameUserIds()
    {

        //Arrange
        var sameId = Guid.NewGuid();
        var createModel = Command.Create.New(sameId, sameId);

        //Act
        var result = await handler.Handle(createModel, cancellationToken);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(ErrorResults.Canceled("You can not chat with yourself!"));
    }

    [Fact]
    public async Task Handle_Should_Return_NotFound_For_Missing_Receiver()
    {
        // Arrange
        var requesterId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        var request = Command.Create.New(requesterId, receiverId);
        AppUser? receiverUser = null; // must be null => receiver not exist in database.

        _mocker.Setup(x => x.Queries.Users.FindByIdAsync(receiverId))
            .ReturnsAsync(receiverUser);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(ErrorResults.NotFound($"The receiver with ID : <{receiverId}> Not Found."));
        _mocker.Verify(x => x.Queries.Users.FindByIdAsync(receiverId), Times.Once());
    }

    [Fact]
    public async Task Handle_Should_Return_Founded_For_Existing_ChatItem()
    {
        // Arrange
        var requesterId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        var request = Command.Create.New(requesterId, receiverId);
        var item = ChatItem.Create(requesterId, receiverId);
        var receiver = AppUser.Create(new() { Id = receiverId });

        _mocker.Setup(x => x.Queries.Users.FindByIdAsync(receiverId))
             .ReturnsAsync(receiver);

        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(requesterId, receiverId))
          .ReturnsAsync(item);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(ErrorResults.Founded($"The <Chat-Item> with ID : <{item.Id}> was found."));
        _mocker.Verify(x => x.Queries.Users.FindByIdAsync(receiverId), Times.Once());
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdsAsync(requesterId, receiverId), Times.Once());
    }

    [Fact]
    public async Task Handle_Should_Create_ChatItem_Successfully()
    {
        // Arrange
        var requesterId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        var request = Command.Create.New(requesterId, receiverId);
        var receiver = AppUser.Create(new() { Id = receiverId });
        ChatItem? existingItem = null;

        _mocker.Setup(x => x.Queries.Users.FindByIdAsync(receiverId))
             .ReturnsAsync(receiver);

        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(requesterId, receiverId))
          .ReturnsAsync(existingItem);

        _mocker.Setup(x => x.CreateAsync(It.IsAny<ChatItem>())).Returns(Task.CompletedTask);


        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(SuccessResults.Ok("The new Chat-Item has been created successfully."));
        _mocker.VerifyAll();
    }
}
