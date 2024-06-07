using Apps.Chats.Abstractions;
using Domains.Chat.MessageAggregate;
using Infra.EFCore.Contexts;

namespace Infra.EFCore.Implementations.Chats;
internal class MessageQueries(AppDbContext _dbContext) : IMessageQueries {
    public async Task<ChatMessage?> FindByIdAsync(Guid messageId) {
        return await _dbContext.ChatMessages.FindAsync(messageId);
    }
}
