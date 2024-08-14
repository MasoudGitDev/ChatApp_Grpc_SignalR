using Apps.Chats.Shared.Queries;
using Domains.Auth.User.Aggregate;
using FluentAssertions;
using Moq;
using UnitOfWorks.Abstractions;
using Query = Apps.Chats.Shared.Queries;

namespace UNTests.Apps.Chats.Shared.Queries;
public class FindMeByUserName {

    private readonly Mock<IChatUOW> _mocker;
    private readonly FindMeByUserNameHandler _handler;
    private readonly CancellationToken _cancellationToken = new();
    public FindMeByUserName() {
        _mocker = new Mock<IChatUOW>();
        _handler = new(_mocker.Object);
    }

    [Fact]
    public async Task Handle_Should_Find_AuthenticatedUser_By_UserName() {

        //Arrange
        var request = Query.FindMeByUserName.New("test-userName");
        AppUser authenticatedUser = AppUser.Create(new(){
            Id = Guid.NewGuid(),
            DisplayName = "test-display-name",
            Email = "test-email" ,
            Password = "test-password" ,
            UserName = "test-userName"
        });
        _mocker.Setup(x => x.Queries.Users.FindByUserNameAsync(request.UserName))
            .ReturnsAsync(authenticatedUser);

        //Act
        var result = await _handler.Handle(request,_cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(authenticatedUser);
        _mocker.Verify(x => x.Queries.Users.FindByUserNameAsync(request.UserName) , Times.Once);

    }

    [Fact]
    public async Task Handle_Should_Return_InvalidUser_When_UserName_Is_Invalid() {

        //Arrange
        var request = Query.FindMeByUserName.New("test-userName");
        AppUser authenticatedUser = AppUser.InvalidUser;
        _mocker.Setup(x => x.Queries.Users.FindByUserNameAsync(request.UserName))
            .ReturnsAsync(authenticatedUser);

        //Act
        var result = await _handler.Handle(request,_cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(authenticatedUser);
        _mocker.Verify(x => x.Queries.Users.FindByUserNameAsync(request.UserName) , Times.Once);

    }

}
