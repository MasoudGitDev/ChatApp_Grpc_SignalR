using Domains.Auth.User.Aggregate;

namespace Domains.Chats.Contacts.Aggregate;
public partial record class Contact {
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid RequesterId { get; private set; }
    public Guid ReceiverId { get; private set; }

    public DateTime ContactedAt { get; private set; } = DateTime.UtcNow;
}


public partial record class Contact {
    public static Contact Create(Guid requesterId , Guid receiverId) {
        return new() {
            ReceiverId = receiverId,
            RequesterId = requesterId,
        };
    }


    public bool IsInContact(Guid userId) {
        if(ReceiverId == userId || RequesterId == userId) {
            return true;
        }
        return false;
    }

}