using Domains.Auth.User.Aggregate;
using Domains.Chats.Contacts.Aggregate;
using Domains.Chats.Contacts.Queries;
using Infra.EFCore.Contexts;
using Infra.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Server.Dtos.Dashboard;

namespace Infra.EFCore.Implementations.Chats;
internal sealed class ContactQueries(AppDbContext _dbContext) : IContactQueries {
    public async Task<Contact?> FindAsync(Guid contactId) {
        return await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == contactId);
    }

    public async Task<Contact?> FindByUserIdAsync(Guid userId) {
        return await _dbContext.Contacts.FirstOrDefaultAsync(x => x.ReceiverId == userId || x.RequesterId == userId);
    }

    public async Task<LinkedList<ContactItemDto>> GetContacts(Guid userId) {
        return await _dbContext.Contacts
            .Where(x => x.ReceiverId == userId || x.RequesterId == userId)
            .ToContactItemsAsync(userId , FindUserByIdAsync);
    }

    public async Task<Contact?> IsInContactAsync(Guid userId1 , Guid userId2) {
        return await _dbContext.Contacts
             .Where(x => x.ReceiverId == userId1 || x.ReceiverId == userId2)
             .Where(x => x.RequesterId == userId1 || x.RequesterId == userId2)
             .FirstOrDefaultAsync();
    }

    private async Task<AppUser> FindUserByIdAsync(Guid userId)
        => await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? AppUser.InvalidUser;
}
