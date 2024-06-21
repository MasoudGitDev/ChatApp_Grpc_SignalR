using Apps.Chats.Queries;
using Domains.Auth.User.Aggregate;
using Domains.Chats.Requests.Aggregate;

namespace Infra.EFCore.Implementations.Chats {
    internal sealed class ChatRequestQueries : IChatRequestQueries {
        public Task<ChatRequest?> BlockAsync(Guid receiverId) {
            throw new NotImplementedException();
        }

        public Task<AppUser?> FindAsync(Guid userId) {
            throw new NotImplementedException();
        }
    }
}
