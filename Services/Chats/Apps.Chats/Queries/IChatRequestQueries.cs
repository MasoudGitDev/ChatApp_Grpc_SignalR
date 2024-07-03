using Domains.Chats.Requests.Aggregate;

namespace Apps.Chats.Queries;
public interface IChatRequestQueries {
    Task<ChatRequest?> FindByIdAsync(Guid chatRequestId);
    Task<ChatRequest?> FindSameRequestAsync(Guid userId1 , Guid userId2);
    Task<LinkedList<ChatRequest>> GetReceiveRequestsAsync(Guid receiverId);
    Task<LinkedList<ChatRequest>> GetSendRequestsAsync(Guid requesterId);
}
