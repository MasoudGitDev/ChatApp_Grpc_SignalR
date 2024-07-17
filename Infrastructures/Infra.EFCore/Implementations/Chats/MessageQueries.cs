using Domains.Chats.Message.Aggregate;
using Domains.Chats.Message.Queries;
using Infra.EFCore.Contexts;

namespace Infra.EFCore.Implementations.Chats;
internal class MessageQueries(AppDbContext _dbContext) : IChatMessageQueries {
    public async Task<ChatMessage?> FindByIdAsync(Guid messageId) {
        return await _dbContext.ChatMessages.FindAsync(messageId);
    }
}
