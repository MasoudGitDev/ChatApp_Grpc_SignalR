using Apps.Chats.Queries;
using Domains.Auth.User.Aggregate;
using Domains.Chats.Requests.Aggregate;
using Infra.EFCore.Contexts;
using Infra.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Server.Models.Results;

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

    public async Task<List<ChatRequestItem>> GetSendRequestsAsync(Guid requesterId) {
        return await _dbContext.ChatRequests
          .Where(x => x.RequesterId == requesterId)
          .ToChatRequestItemsAsync(FindUserByIdAsync , false) ?? [];
    }
    public async Task<List<ChatRequestItem>> GetReceiveRequestsAsync(Guid receiverId) {
        return await _dbContext.ChatRequests
           .Where(x => x.ReceiverId == receiverId)
           .ToChatRequestItemsAsync(FindUserByIdAsync , true) ?? [];
    }
    private async Task<AppUser> FindUserByIdAsync(Guid userId) {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? AppUser.Empty;
    }

}