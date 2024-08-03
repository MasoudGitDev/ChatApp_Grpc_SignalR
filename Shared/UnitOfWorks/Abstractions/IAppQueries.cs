using Domains.Auth.Online.Queries;
using Domains.Auth.Queries;
using Domains.Chats.Item.Queries;
using Domains.Chats.Message.Queries;

namespace UnitOfWorks.Abstractions;

public interface IAppQueries {
    IUserQueries Users { get; }
    IChatItemQueries ChatItems { get; }
    IChatMessageQueries ChatMessages { get; }
    IOnlineUserQueries OnlineUsers { get; }
}

