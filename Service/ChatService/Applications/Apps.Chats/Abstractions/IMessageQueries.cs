using Domains.Chat.MessageAggregate;

namespace Apps.Chats.Abstractions;
public interface IMessageQueries {
    Task<ChatMessage?> FindByIdAsync(Guid messageId);

}
