using Apps.Chats.ChatItems.Queries;
using Domains.Auth.User.Aggregate;
using Domains.Chats.Item.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Server.Dtos.Chat;
using Shared.Server.Models.Results;
using UnitOfWorks.Abstractions;
using QueryModels = Apps.Chats.ChatItems.Queries;

namespace UNTests.Apps.Chats.ChatItems.Queries;
public class GetCloudItem {

    private readonly Mock<IChatUOW> _mocker;
    private readonly GetCloudItemHandler _handler;
    private readonly CancellationToken CancellationToken = new();

    public GetCloudItem() {
        _mocker = new Mock<IChatUOW>();
        _handler = new(_mocker.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_NotFound_When_MyId_IsNotValid() {
        //Arrange
        QueryModels.GetCloudItem request = QueryModels.GetCloudItem.New(Guid.NewGuid());
        _mocker.Setup(x => x.Queries.Users.FindByIdAsync(request.MyId))
            .ReturnsAsync((AppUser?) null);

        //Act
        var result = await _handler.Handle(request,CancellationToken);

        //Assert
        result.Should().BeEquivalentTo(
            ErrorResults.NotFound<ChatItemDto>($"The user with ID : <{request.MyId}> has not been found."));
        _mocker.Verify(x => x.Queries.Users.FindByIdAsync(request.MyId) , Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_CloudItem_When_EveryThingIsOK() {
        //Arrange
        Guid MyId = Guid.NewGuid();
        QueryModels.GetCloudItem request = QueryModels.GetCloudItem.New(MyId);
        AppUser authenticatedUser = AppUser.Create(new(){
            DisplayName = "Test-DisplayName" ,
            Email = "Test-Email" ,
            Id = MyId,
            Password = "Test-Password" ,
            UserName = "Test-UserName"
        });
        _mocker.Setup(x => x.Queries.Users.FindByIdAsync(request.MyId))
            .ReturnsAsync(authenticatedUser);

        ChatItem chatItem = ChatItem.Create(request.MyId,request.MyId);
        _mocker.Setup(x => x.Queries.ChatItems.FindByIdsAsync(request.MyId , request.MyId))
            .ReturnsAsync(chatItem);

        ChatItemDto expectedResult = new(){
            DisplayName = authenticatedUser.DisplayName ,
            ReceiverId = authenticatedUser.Id ,
            LogoUrl = authenticatedUser.ImageUrl ,
            UnReadMessages = 0 ,
            Id = chatItem.Id, // means ChatItemId must be not null!
        };

        //Act
        var result = await _handler.Handle(request,CancellationToken);

        //Assert
        result.Should().BeEquivalentTo(SuccessResults.Ok(expectedResult));
        _mocker.Verify(x => x.Queries.Users.FindByIdAsync(request.MyId) , Times.Once);
        _mocker.Verify(x => x.Queries.ChatItems.FindByIdsAsync(request.MyId , request.MyId) , Times.Once);
    }


}
