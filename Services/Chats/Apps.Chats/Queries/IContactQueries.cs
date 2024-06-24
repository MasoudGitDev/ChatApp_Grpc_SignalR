using Domains.Chats.Contacts.Aggregate;

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
}
