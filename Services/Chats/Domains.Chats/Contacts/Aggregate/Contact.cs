using Domains.Auth.User.Aggregate;

namespace Domains.Chats.Contacts.Aggregate;
public partial record class Contact {
    public Guid RequesterId { get; private set; }
    public Guid ReceiverId { get; private set; }

    public DateTime ContactedAt { get; private set; } = DateTime.UtcNow;

    //===============relations
    public AppUser Requester { get; private set; } = null!;
    public AppUser Receiver { get; private set; } = null!;
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