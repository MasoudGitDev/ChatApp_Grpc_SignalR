using Domains.Chats.Item.Aggregate;

namespace Domains.Chats.Item.Queries;
public interface IChatItemQueries {
    public Task<ChatItem?> FindByIdAsync(Guid chatId);
    public Task<ChatItem?> FindByIdsAsync(Guid requesterId , Guid receiverId);
    public Task<List<ChatItem>> GetItemsByUserIdAsync(Guid userId , int pageNumber = 1 , int pageSize = 20);
    public Task<bool> HaveAnyChatItem(Guid requesterId , Guid receiverId);
}
