using Domains.Auth.User.Aggregate;
using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.ValueObjects;

namespace Domains.Chats.Message.Aggregate;
public partial class ChatMessage {
    public Guid Id { get; private set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; private set; }

    public FileUrl FileUrl { get; private set; }
    public string Content { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; private set; } = null;

    //=========================relations
    public ChatItem Chat { get; private set; } = null!;
    public AppUser Sender { get; private set; } = null!;
}

//Methods
public partial class ChatMessage {

    public static ChatMessage Create(
        Guid chatId ,
        Guid senderId ,
        string content ,
        FileUrl? fileUrl = null ,
        Guid? messageId = null) => new() {
            ChatId = chatId ,
            SenderId = senderId ,
            Id = messageId ?? Guid.NewGuid() ,
            Content = content ,
            FileUrl = fileUrl ?? FileUrl.Empty
        };

    public void Update(FileUrl fileUrl) {
        FileUrl = fileUrl;
        ModifiedAt = DateTime.UtcNow;
    }
    public void Update(string content) {
        Content = content;
        ModifiedAt = DateTime.UtcNow;
    }
}
