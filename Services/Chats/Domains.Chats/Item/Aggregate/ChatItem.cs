using Domains.Auth.User.Aggregate;
using Domains.Chats.Message.Aggregate;
using Domains.Chats.Message.ValueObjects;

namespace Domains.Chats.Item.Aggregate;
public partial class ChatItem {
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid RequesterId { get; private set; }
    public Guid ReceiverId { get; private set; }

    public bool IsHiddenForRequester { get; private set; } = false;
    public bool IsHiddenForReceiver { get; private set; } = false;

    public bool IsBlockedByRequester { get; private set; }
    public bool IsBlockedByReceiver { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public LinkedList<ChatMessage> Messages { get; private set; } = new();

    //========================relations
    public AppUser Requester { get; set; } = null!;
    public AppUser Receiver { get; set; } = null!;


}

// Operations
public partial class ChatItem {
    public static ChatItem Create(Guid requesterId , Guid receiverId) => new() {
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


    public void SendMessage(Guid senderId , string content , FileUrl? fileUrl = null) {
        Messages.AddLast(ChatMessage.Create(Guid.NewGuid() , senderId , content , fileUrl));
    }

}
