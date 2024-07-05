using Domains.Chats.Contacts.Aggregate;
using Shared.Server.Dtos.Dashboard;

namespace Apps.Chats.Queries;
public interface IContactQueries {
  
    Task<Contact?> FindAsync(Guid contactId);

    /// <summary>
    /// The userId can be receiverId or requesterId.
    /// </summary>
    Task<Contact?> FindByUserIdAsync(Guid userId);

    /// <summary>
    /// Each one of userId1 or userId2 can be receiverId or requesterId.
    /// </summary>
    Task<Contact?> IsInContactAsync(Guid userId1 , Guid userId2);

    Task<LinkedList<ContactItemDto>> GetContacts(Guid userId);
}
