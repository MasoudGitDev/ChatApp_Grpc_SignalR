using Domains.Chats.Item.Aggregate;
using Domains.Chats.Item.Queries;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace UNTests.Domains.Chats.ChatItems;
public class QueriesTests {

    private readonly AutoMocker _mocker;
    private readonly Mock<IChatItemQueries> _mockQueries;

    public QueriesTests() {
        _mocker = new AutoMocker();
        _mockQueries = _mocker.GetMock<IChatItemQueries>();
    }

    [Fact]
    public async Task FindById_Should_Return_Valid_ChatItem() {
        //Arrange
        ChatItem item = CreateChatItem();
        _mocker.GetMock<IChatItemQueries>()
            .Setup(x => x.FindByIdAsync(item.Id))
            .ReturnsAsync(item);

        //Act
        var findItem = await _mockQueries.Object.FindByIdAsync(item.Id);

        //Assert
        findItem.Should().NotBeNull();
        findItem.Id.Should().Be(item.Id);
        findItem.RequesterId.Should().Be(item.RequesterId);
        findItem.ReceiverId.Should().Be(item.ReceiverId);
    }


    [Fact]
    public async Task FindByIds_Should_Return_Valid_ChatItem() {
        //Arrange
        ChatItem item = CreateChatItem();
        _mocker.GetMock<IChatItemQueries>()
            .Setup(x => x.FindByIdsAsync(item.RequesterId , item.ReceiverId))
            .ReturnsAsync(item);

        //Act
        var findItem = await _mockQueries.Object.FindByIdsAsync(item.RequesterId,item.ReceiverId);

        //Assert
        findItem.Should().NotBeNull();
        findItem.Id.Should().Be(item.Id);
        findItem.RequesterId.Should().Be(item.RequesterId);
        findItem.ReceiverId.Should().Be(item.ReceiverId);
    }


    [Theory]
    [InlineData(1 , 20)]
    [InlineData(2 , 5)]
    public async Task GetItemsByUserId_Should_Return_Items_When_UserId_Is_Requester(int pageNumber , int pageSize) {

        //Arrange
        var userId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");
        List<ChatItem> fakeItems = CreateFakeItemsByUserId(
            requesterId : userId,
            receiverId : Guid.NewGuid() ,
            max : 10);
        fakeItems.AddRange(CreateFakeItems());

        //---------check RequesterId
        var expectedItems = fakeItems
            .Where(x=>x.RequesterId == userId)
            .Skip((pageNumber-1)*pageSize)
            .Take(pageSize)
            .ToList();

        _mocker.GetMock<IChatItemQueries>()
            .Setup(x => x.GetItemsByUserIdAsync(userId , pageNumber , pageSize))
            .ReturnsAsync(expectedItems);

        //Act
        var resultItems = await _mockQueries.Object.GetItemsByUserIdAsync (userId , pageNumber,pageSize);

        //Assert
        resultItems.Should().NotBeNull();
        _mocker.GetMock<IChatItemQueries>()
            .Verify(x => x.GetItemsByUserIdAsync(userId , pageNumber , pageSize) , Times.Once);
        resultItems.Count.Should().Be(expectedItems.Count);
    }

    [Theory]
    [InlineData(1 , 20)]
    [InlineData(2 , 5)]
    public async Task GetItemsByUserId_Should_Return_Items_When_UserId_Is_Receiver(int pageNumber , int pageSize) {

        //Arrange
        var userId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");
        List<ChatItem> fakeItems = CreateFakeItemsByUserId(
            requesterId : Guid.NewGuid() ,
            receiverId : userId,
            max : 10);
        fakeItems.AddRange(CreateFakeItems());

        //---------check receiverId
        var expectedItems = fakeItems
            .Where(x=>x.ReceiverId == userId)
            .Skip((pageNumber-1)*pageSize)
            .Take(pageSize)
            .ToList();

        _mocker.GetMock<IChatItemQueries>()
            .Setup(x => x.GetItemsByUserIdAsync(userId , pageNumber , pageSize))
            .ReturnsAsync(expectedItems);

        //Act
        var resultItems = await _mockQueries.Object.GetItemsByUserIdAsync (userId , pageNumber,pageSize);

        //Assert
        resultItems.Should().NotBeNull();
        resultItems.Count.Should().Be(expectedItems.Count);
        _mocker.GetMock<IChatItemQueries>()
           .Verify(x => x.GetItemsByUserIdAsync(userId , pageNumber , pageSize) , Times.Once);
    }

    //================================ 
    private static ChatItem CreateChatItem()
        => ChatItem.Create(Guid.NewGuid() , Guid.NewGuid() , Guid.NewGuid());

    private List<ChatItem> CreateFakeItems() {
        var items = new List<ChatItem>();
        for(int i = 1 ; i <= 10 ; i++) {
            items.Add(ChatItem.Create(Guid.NewGuid() , Guid.NewGuid()));
        }
        return items;
    }

    private List<ChatItem> CreateFakeItemsByUserId(Guid requesterId , Guid receiverId , int max = 10) {
        var items = new List<ChatItem>();
        for(int i = 1 ; i <= max ; i++) {
            items.Add(ChatItem.Create(requesterId , receiverId));
        }
        return items;
    }
}
