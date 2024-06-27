using Apps.Chats.Queries;
using Domains.Chats.Contacts.Aggregate;
using Infra.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.EFCore.Implementations.Chats;
internal sealed class ContactQueries(AppDbContext _dbContext) : IContactQueries {
    public async Task<Contact?> FindAsync(Guid contactId) {
        return await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == contactId);
    }

    public async Task<Contact?> FindByUserIdAsync(Guid userId) {
        return await _dbContext.Contacts.FirstOrDefaultAsync(x => x.ReceiverId == userId || x.RequesterId == userId);
    }
    public async Task<Contact?> IsInContactAsync(Guid userId1 , Guid userId2) {
        return await _dbContext.Contacts
             .Where(x => x.ReceiverId == userId1 || x.ReceiverId == userId2 )
             .Where(x => x.RequesterId == userId1 || x.RequesterId == userId2)
             .FirstOrDefaultAsync();
    }
}
