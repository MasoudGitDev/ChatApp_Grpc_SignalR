using Apps.Auth.Queries;

namespace Apps.Chats.Queries;

public interface IAppQueries {
    IUserQueries Users { get; }
    IChatRequestQueries ChatRequests { get; }
    IChatItemQueries ChatItems { get; }
    IChatMessageQueries ChatMessages { get; }
    IContactQueries Contacts { get; }
}
