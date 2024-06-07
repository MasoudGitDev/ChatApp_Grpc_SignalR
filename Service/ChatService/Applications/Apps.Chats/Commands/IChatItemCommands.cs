using Shared.Server.Models.Results;

namespace Apps.Chats.Commands;

public interface IChatItemCommands
{
    Task<Result> CreateAsync(Guid requesterId, Guid receiverId);
    Task<Result> DeleteAsync(Guid chatId);
}
