using Domains.Auth.Online.Queries;
using Domains.Auth.User.Queries;
using Domains.Chats.Item.Queries;
using Domains.Chats.Message.Queries;
using UnitOfWorks.Abstractions;

namespace Infra.SqlServerWithEF.Implementations;
internal sealed class AppQueries(
    IUserQueries _users ,
    IChatItemQueries _chatItems ,
    IChatMessageQueries _messages ,
    IOnlineUserQueries _onlineUsers
    ) : IAppQueries {
    public IUserQueries Users => _users;
    public IChatItemQueries ChatItems => _chatItems;
    public IChatMessageQueries ChatMessages => _messages;
    public IOnlineUserQueries OnlineUsers => _onlineUsers;
}
