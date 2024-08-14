using Domains.Chats.Message.Aggregate;

namespace Domains.Chats.Message.Queries;
public interface IChatMessageQueries {
    Task<ChatMessage?> FindByIdAsync(Guid messageId);
    Task<List<ChatMessage>> GetAllAsync(Guid chatItemId ,bool usePagination = false  , int pageNumber = 1 , int pageSize = 50);
    Task<List<ChatMessage>> GetUnreadMessagesAsync(Guid chatItemId);
}
