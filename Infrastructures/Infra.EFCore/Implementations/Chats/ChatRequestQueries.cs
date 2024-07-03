using Apps.Chats.Queries;
using Domains.Chats.Requests.Aggregate;
using Infra.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Server.Extensions;

namespace Infra.EFCore.Implementations.Chats;
internal sealed class ChatRequestQueries(AppDbContext _dbContext) : IChatRequestQueries {
    public async Task<ChatRequest?> FindByIdAsync(Guid chatRequestId) {
        return await _dbContext.ChatRequests.FirstOrDefaultAsync(x => x.Id == chatRequestId);
    }

    public async Task<ChatRequest?> FindSameRequestAsync(Guid userId1 , Guid userId2) {
        return await _dbContext.ChatRequests
            .Where(x => x.RequesterId == userId1 || x.RequesterId == userId2)
            .Where(x => x.ReceiverId == userId1 || x.ReceiverId == userId2)
            .FirstOrDefaultAsync();
    }

    public async Task<LinkedList<ChatRequest>> GetReceiveRequestsAsync(Guid receiverId) {
        return await _dbContext.ChatRequests
            .Where(x => x.ReceiverId == receiverId)
            .ToLinkedListAsync();
    }

    public async Task<LinkedList<ChatRequest>> GetSendRequestsAsync(Guid requesterId) {
        return await _dbContext.ChatRequests
        .Where(x => x.RequesterId == requesterId)
        .ToLinkedListAsync();
    }
}