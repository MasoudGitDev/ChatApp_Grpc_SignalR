using Domains.Auth.User.Aggregate;
using Domains.Chats.Requests.Aggregate;

namespace Apps.Chats.Queries {
   public interface IChatRequestQueries {
        Task<ChatRequest?> FindByIdAsync(Guid chatRequestId);
    }
}
