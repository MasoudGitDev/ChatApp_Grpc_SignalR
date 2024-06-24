using Apps.Chats.Queries;
using Domains.Chats.Requests.Aggregate;
using Infra.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.EFCore.Implementations.Chats;
internal sealed class ChatRequestQueries(AppDbContext _dbContext) : IChatRequestQueries {
    public async Task<ChatRequest?> FindByIdAsync(Guid chatRequestId) {
        return await _dbContext.ChatRequests.FirstOrDefaultAsync(x => x.Id == chatRequestId);
    }
}