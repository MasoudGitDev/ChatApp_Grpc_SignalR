using Domains.Auth.Queries;
using Domains.Chats.Contacts.Queries;
using Domains.Chats.Item.Queries;
using Domains.Chats.Message.Queries;
using Domains.Chats.Requests.Queries;

namespace UnitOfWorks.Abstractions;

public interface IAppQueries {
    IUserQueries Users { get; }
    IChatRequestQueries ChatRequests { get; }
    IChatItemQueries ChatItems { get; }
    IChatMessageQueries ChatMessages { get; }
    IContactQueries Contacts { get; }
}

