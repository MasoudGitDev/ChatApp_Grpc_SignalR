using Apps.Auth.Queries;
using Apps.Chats.Queries;

namespace Infra.EFCore.Implementations;
internal sealed class AppQueries(
    IUserQueries _users ,
    IChatItemQueries _chatItems ,
    IChatMessageQueries _messages ,
    IChatRequestQueries _requests ,
    IContactQueries _contacts
    ) : IAppQueries {
    public IUserQueries Users => _users;
    public IChatRequestQueries ChatRequests => _requests;
    public IChatItemQueries ChatItems => _chatItems;
    public IChatMessageQueries ChatMessages => _messages;
    public IContactQueries Contacts => _contacts;
}
