using Domains.Chats.Requests.Aggregate;
using Shared.Server.Models.Results;

namespace Domains.Chats.Requests.Queries;
public interface IChatRequestQueries {
    Task<ChatRequest?> FindByIdAsync(Guid chatRequestId);
    Task<ChatRequest?> FindSameRequestAsync(Guid userId1 , Guid userId2);
    Task<List<ChatRequestItem>> GetReceiveRequestsAsync(Guid receiverId);
    Task<List<ChatRequestItem>> GetSendRequestsAsync(Guid requesterId);
}
