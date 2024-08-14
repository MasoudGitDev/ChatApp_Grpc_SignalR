using Domains.Chats.Message.Aggregate;

namespace Domains.Chats.Item.Aggregate;
public partial class ChatItem {
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid RequesterId { get; private set; }
    public Guid ReceiverId { get; private set; }

    public bool IsHiddenForRequester { get; private set; } = false;
    public bool IsHiddenForReceiver { get; private set; } = false;

    public bool IsBlockedByRequester { get; private set; } = false;
    public bool IsBlockedByReceiver { get; private set; } = false;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public LinkedList<ChatMessage> Messages { get; private set; } = new();
}

// Operations
public partial class ChatItem {
    public static ChatItem Create(Guid requesterId , Guid receiverId) => new() {
        RequesterId = requesterId ,
        ReceiverId = receiverId
    };

    public static ChatItem Create(Guid Id , Guid requesterId , Guid receiverId) => new() {
        Id = Id ,
        RequesterId = requesterId ,
        ReceiverId = receiverId
    };

    public void Hide(bool forRequester) {
        if(forRequester) {
            IsHiddenForRequester = true;
        }
        else {
            IsHiddenForReceiver = true;
        }
    }

    public void Block(bool isBlockedByRequester) {
        if(isBlockedByRequester) {
            IsBlockedByRequester = true;
        }
        else {
            IsBlockedByReceiver = true;
        }
    }

}
