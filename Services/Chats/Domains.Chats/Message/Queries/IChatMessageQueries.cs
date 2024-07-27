using Domains.Chats.Message.Aggregate;

namespace Domains.Chats.Message.Queries;
public interface IChatMessageQueries {
    Task<ChatMessage?> FindByIdAsync(Guid messageId);
    Task<List<ChatMessage>> GetAllAsync(Guid chatItemId);

}
