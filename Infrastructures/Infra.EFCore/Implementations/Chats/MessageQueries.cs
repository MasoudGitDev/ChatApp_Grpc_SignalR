using Domains.Chats.Message.Aggregate;
using Domains.Chats.Message.Queries;
using Infra.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.EFCore.Implementations.Chats;
internal class MessageQueries(AppDbContext _dbContext) : IChatMessageQueries {
    public async Task<ChatMessage?> FindByIdAsync(Guid messageId) {
        return await _dbContext.ChatMessages.FindAsync(messageId);
    }

    public async Task<List<ChatMessage>> GetAllAsync(Guid chatItemId , int pageNumber = 1 , int pageSize = 50) {
        return await _dbContext.ChatMessages
            .Where(x => x.ChatItemId == chatItemId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(x=> x.CreatedAt)            
            .ToListAsync();
    }
}
