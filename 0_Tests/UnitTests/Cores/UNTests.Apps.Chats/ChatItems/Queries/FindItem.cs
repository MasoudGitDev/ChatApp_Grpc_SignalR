using Apps.Chats.ChatItems.Queries;
using Domains.Chats.Item.Aggregate;
using FluentAssertions;
using Mapster;
using Moq;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;

namespace UNTests.Apps.Chats.ChatItems.Queries;
public class FindItem {

    private readonly Mock<IChatUOW> _mocker;
    private readonly FindChatItemHandler _handler;
    private readonly CancellationToken CancellationToken = new();

    public FindItem() {
        _mocker = new Mock<IChatUOW>();
        _handler = new(_mocker.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_OkResult_When_ChatItem_Exist() {

        //Arrange
        FindChatItem request = FindChatItem.New(Guid.NewGuid() , Guid.NewGuid());
        ChatItem expectedItem = ChatItem.Create(request.MyId,request.OtherId);
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(request.MyId , request.OtherId))
            .ReturnsAsync(expectedItem);

        //Act
        var result = await _handler.Handle(request,CancellationToken);

        //Assert
        result.Should().BeEquivalentTo(SuccessResults.Ok(expectedItem.Adapt<ChatItemDto>()));
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdsAsync(request.MyId , request.OtherId) , Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_OkResult_When_ChatItem_NotExist() {

        //Arrange
        FindChatItem request = FindChatItem.New(Guid.NewGuid() , Guid.NewGuid());
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(request.MyId , request.OtherId))
            .ReturnsAsync((ChatItem?) null);
        ChatItemDto expectedItem = new() {
            DisplayName = "Test" ,
            Id = Guid.NewGuid(),
            ReceiverId = request.OtherId ,
            LogoUrl = "img-test" ,
            UnReadMessages = 0
        };

        //Act
        var result = await _handler.Handle(request,CancellationToken);

        //Assert
        result.Model.Should().NotBeNull();
        result.Should().BeOfType<ResultStatus<ChatItemDto>>();
        result.Model.DisplayName.Should().Be(expectedItem.DisplayName); // why Id is change ?
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdsAsync(request.MyId , request.OtherId) , Times.Once);

    }

}
