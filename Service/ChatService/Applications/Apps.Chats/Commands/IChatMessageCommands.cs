using Domains.Chat.MessageAggregate;
using Shared.Server.Models.Results;

namespace Apps.Chats.Commands.Impls;

public interface IChatMessageCommands {
    Task<Result> SendAsync(ChatMessage chatMessage);
    Task<Result> DeleteAsync(Guid messageId);
}
