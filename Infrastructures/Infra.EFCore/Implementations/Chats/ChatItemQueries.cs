using Apps.Chats.Queries;
using Domains.Chats.Item.Aggregate;
using Infra.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Server.Extensions;

namespace Infra.EFCore.Implementations.Chats;
internal class ChatItemQueries(AppDbContext _dbContext) : IChatItemQueries {
    public async Task<ChatItem?> FindByIdAsync(Guid chatId)
        => await _dbContext.ChatItems.FirstOrDefaultAsync(x => x.Id == chatId);

    public async Task<ChatItem?> GetByIdsAsync(Guid requesterId , Guid receiverId)
         => await _dbContext.ChatItems.FirstOrDefaultAsync(x => x.RequesterId == requesterId && x.ReceiverId == receiverId);

    public async Task<LinkedList<ChatItem>> GetByIdAsync(Guid userId)
          => await _dbContext.ChatItems.Where(x => x.ReceiverId == userId || x.RequesterId == userId).ToLinkedListAsync();

    public async Task<bool> HaveAnyChatItem(Guid requesterId , Guid receiverId)
        => await _dbContext.ChatItems.AnyAsync(item =>
            ( item.RequesterId == requesterId && item.ReceiverId == receiverId ) ||
            ( item.RequesterId == receiverId && item.ReceiverId == requesterId )
        );
}
