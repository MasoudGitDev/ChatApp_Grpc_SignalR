using Shared.Server.Extensions;

namespace Domains.Chat.ChatAggregate.ValueObjects;
internal class ChatId {
    public Guid RequesterId { get; private set; }
    public Guid ReceiverId { get; private set; }
    public (Guid requesterId, Guid receiverId) Value => (RequesterId, ReceiverId);
   
    public ChatId() {

    }
    public static ChatId Create() => new() {
        RequesterId = Guid.NewGuid() ,
        ReceiverId = Guid.NewGuid() ,
    };
    public static ChatId Create(Guid requesterId , Guid receiverId) {
        if(requesterId == Guid.Empty || receiverId == Guid.Empty) {
            throw new Exception("Invalid ChatId");
        }
        return new() {
            RequesterId = requesterId ,
            ReceiverId = receiverId ,
        };
    }
    public static implicit operator string(ChatId chatId) => $"{chatId.RequesterId}:{chatId.ReceiverId}";
    public static implicit operator ChatId(string chatId) {
        string[] ids = chatId.Split(':') ;
        return new() { RequesterId = (ids[0]).AsGuid() , ReceiverId = ( ids[1].AsGuid()) };
    }

}
