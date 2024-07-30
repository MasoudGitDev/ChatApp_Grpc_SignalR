using Domains.Chats.Item.Aggregate;
using Domains.Chats.Item.Queries;
using Infra.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.EFCore.Implementations.Chats;
internal class ChatItemQueries(AppDbContext _dbContext) : IChatItemQueries {
    public async Task<ChatItem?> FindByIdAsync(Guid chatId)
        => await _dbContext.ChatItems.FirstOrDefaultAsync(x => x.Id == chatId);

    public async Task<ChatItem?> GetByIdsAsync(Guid requesterId , Guid receiverId)
         => await _dbContext.ChatItems.FirstOrDefaultAsync(item =>
            ( item.RequesterId == requesterId && item.ReceiverId == receiverId ) ||
            ( item.RequesterId == receiverId && item.ReceiverId == requesterId )
         );

    public async Task<List<ChatItem>> GetByIdAsync(Guid userId , int pageNumber = 1 , int pageSize = 20)
        => await _dbContext.ChatItems
        .Where(x => x.ReceiverId == userId || x.RequesterId == userId)
        .Skip(( pageNumber - 1 ) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    public async Task<bool> HaveAnyChatItem(Guid requesterId , Guid receiverId)
        => await _dbContext.ChatItems.AnyAsync(item =>
            ( item.RequesterId == requesterId && item.ReceiverId == receiverId ) ||
            ( item.RequesterId == receiverId && item.ReceiverId == requesterId )
        );
}
