using Domains.Chats.Item.Aggregate;

namespace Domains.Chats.Item.Queries;
public interface IChatItemQueries {
    public Task<ChatItem?> FindByIdAsync(Guid chatId);
    public Task<ChatItem?> GetByIdsAsync(Guid requesterId , Guid receiverId);
    public Task<LinkedList<ChatItem>> GetByIdAsync(Guid userId);
    public Task<bool> HaveAnyChatItem(Guid requesterId , Guid receiverId);
}
