using Apps.Chats.Queries;
using Domains.Chats.Contacts.Aggregate;
using Infra.EFCore.Contexts;

namespace Infra.EFCore.Implementations.Chats;
internal sealed class ContactQueries(AppDbContext _dbContext) : IContactQueries {
    public Task<Contact?> FindAsync(Guid contactId) {
        throw new NotImplementedException();
    }

    public Task<Contact?> FindByUserIdAsync(Guid userId) {
        throw new NotImplementedException();
    }

    public Task<Contact?> IsInContactAsync(Guid userId1 , Guid userId2) {
        throw new NotImplementedException();
    }
}
