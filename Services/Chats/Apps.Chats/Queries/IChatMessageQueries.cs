using Domains.Chats.Message.Aggregate;

namespace Apps.Chats.Queries;
public interface IChatMessageQueries {
    Task<ChatMessage?> FindByIdAsync(Guid messageId);

}
