using Domains.Chats.Message.Aggregate;
using Domains.Chats.Message.Queries;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Implementations.Chats;
internal class MessageQueries(AppDbContext _dbContext) : IChatMessageQueries {
    public async Task<ChatMessage?> FindByIdAsync(Guid messageId) {
        return await _dbContext.ChatMessages.FindAsync(messageId);
    }

    public async Task<List<ChatMessage>> GetAllAsync(Guid chatItemId , bool usePagination = false , int pageNumber = 1 , int pageSize = 50) {
        return usePagination ?
            await _dbContext.ChatMessages
            .Where(x => x.ChatItemId == chatItemId)
            .Skip(( pageNumber - 1 ) * pageSize)
            .Take(pageSize)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync() :
            await _dbContext.ChatMessages
            .Where(x => x.ChatItemId == chatItemId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<ChatMessage>> GetUnreadMessagesAsync(Guid chatItemId) {
        return await _dbContext.ChatMessages
           .Where(x => x.ChatItemId == chatItemId && x.IsSeen == false)
           .OrderBy(x => x.CreatedAt)
           .ToListAsync();
    }
}
