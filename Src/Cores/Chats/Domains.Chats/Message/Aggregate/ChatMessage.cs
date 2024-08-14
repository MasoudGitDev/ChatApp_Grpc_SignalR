using Domains.Auth.User.Aggregate;
using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.ValueObjects;
using System.Data;

namespace Domains.Chats.Message.Aggregate;
public partial class ChatMessage {
    public Guid Id { get; private set; }
    public Guid ChatItemId { get; private set; }
    public Guid SenderId { get; private set; }

    public FileUrl FileUrl { get; private set; } = FileUrl.Empty;
    public string Content { get; private set; } = String.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; private set; } = null;

    public bool IsSent { get; private set; } = false;
    public bool IsSeen { get; private set; } = false;

    //=========================relations
    public ChatItem ChatItem { get; private set; } = null!;
    public AppUser Sender { get; private set; } = null!;
}

//Methods
public partial class ChatMessage {

    public static ChatMessage Create(
        ChatItem chatItem ,
        Guid chatItemId ,
        Guid senderId ,
        string content ,
        string fileUrl = "" ,
        Guid? messageId = null) => new() {
            Id = messageId ?? Guid.NewGuid() ,
            ChatItemId = chatItemId ,
            SenderId = senderId ,     
            Content = content ,
            FileUrl = fileUrl ,
            ChatItem = chatItem
    };

    public void Update(FileUrl fileUrl) {
        FileUrl = fileUrl;
        ModifiedAt = DateTime.UtcNow;
    }
    public void Update(string content) {
        Content = content;
        ModifiedAt = DateTime.UtcNow;
    }
    public void MarkAsRead() {
        IsSeen = true;
    }
    public void MarkAsSend() {
        IsSent = true;
    }
}
