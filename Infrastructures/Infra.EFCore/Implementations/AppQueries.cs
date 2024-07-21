using Domains.Auth.Online.Queries;
using Domains.Auth.Queries;
using Domains.Chats.Contacts.Queries;
using Domains.Chats.Item.Queries;
using Domains.Chats.Message.Queries;
using Domains.Chats.Requests.Queries;
using UnitOfWorks.Abstractions;

namespace Infra.EFCore.Implementations;
internal sealed class AppQueries(
    IUserQueries _users ,
    IChatItemQueries _chatItems ,
    IChatMessageQueries _messages ,
    IChatRequestQueries _requests ,
    IContactQueries _contacts ,
    IOnlineUserQueries _onlineUsers
    ) : IAppQueries {
    public IUserQueries Users => _users;
    public IChatRequestQueries ChatRequests => _requests;
    public IChatItemQueries ChatItems => _chatItems;
    public IChatMessageQueries ChatMessages => _messages;
    public IContactQueries Contacts => _contacts;

    public IOnlineUserQueries OnlineUsers => _onlineUsers;
}
