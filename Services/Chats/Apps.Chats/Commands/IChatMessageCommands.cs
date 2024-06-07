using Domains.Chats.Message.Aggregate;
using Shared.Server.Models.Results;

namespace Apps.Chats.Commands;

public interface IChatMessageCommands {
    Task<Result> SendAsync(ChatMessage chatMessage);
    Task<Result> DeleteAsync(Guid messageId);
}
