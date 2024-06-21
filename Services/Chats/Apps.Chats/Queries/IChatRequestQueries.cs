using Domains.Auth.User.Aggregate;
using Domains.Chats.Requests.Aggregate;

namespace Apps.Chats.Queries {
   public interface IChatRequestQueries {

        /// <summary>
        /// The userId can be requester or receiver Id!
        /// </summary>
        public Task<AppUser?> FindAsync(Guid userId);
        public Task<ChatRequest?> BlockAsync(Guid receiverId);
    }
}
